using Logging.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Logging.Interceptors
{
    public class StructuredLoggingAttribute : Attribute, IAsyncActionFilter
    {
        private Log Root = new Log();
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            LogControllerEntry(context);
            var executedAction = await next();
            LogControllerExit(executedAction);
            WriteToFile();
        }
        private void LogControllerEntry(ActionExecutingContext context)
        {
            (string className, string methodName) names = GetNames(context);
            Root.Entry = new LogEntry()
            {
                Time = DateTime.Now,
                Class = names.className,
                Method = names.methodName,
                Input = context.ActionArguments
            };
        }
        private (string className, string methodName) GetNames(ActionExecutingContext context)
        {
            string displayName = context.ActionDescriptor.DisplayName;
            return (displayName.Substring(0, displayName.LastIndexOf('.')),
                    displayName.Substring(displayName.LastIndexOf('.') + 1).Split(" ")[0]);
        }
        private void LogControllerExit(ActionExecutedContext executedAction)
        {
            if (executedAction.Exception == null)
                Root.Exit = new LogExit()
                {
                    Time = DateTime.Now,
                    Output = GetResult(executedAction),
                    HasError = false
                };
            else
                Root.Exit = new LogExit()
                {
                    Time = DateTime.Now,
                    Output = executedAction.Exception,
                    HasError = true
                };
        }
        private void WriteToFile()
        {
            string folderPath = "../Logging/logs";
            string serializedLog = JsonSerializer.Serialize(Root);
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            File.WriteAllText($"{folderPath}/{guid}.json", serializedLog);
        }
        private object GetResult(ActionExecutedContext executedAction)
        {
            switch (executedAction.Result)
            {
                case ObjectResult objectResult:
                    return objectResult.Value;
                case JsonResult jsonResult:
                    return jsonResult.Value;
                case ContentResult contentResult:
                    return contentResult.Content;
                case StatusCodeResult statusCodeResult:
                    return $"Status Code: {statusCodeResult.StatusCode}";
                case RedirectResult redirectResult:
                    return $"Redirect to: {redirectResult.Url}";
                case RedirectToActionResult redirectToActionResult:
                    return $"Redirect to Action: {redirectToActionResult.ActionName}";
                case RedirectToRouteResult redirectToRouteResult:
                    return $"Redirect to Route: {redirectToRouteResult.RouteName}";
                default:
                    var executedActionType = executedAction.Result?.GetType();
                    return $"Case for `{executedActionType}` result type was not implemented!";
            }
        }
    }
}