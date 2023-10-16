using Logging.Configurations;
using Logging.Manager;
using Logging.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logging.Interceptors;
[AttributeUsage(AttributeTargets.Method)]
public class StructuredLoggingAttribute : Attribute, IAsyncActionFilter
{
    private readonly ILog _root;
    public StructuredLoggingAttribute(ILog root) => _root = root;
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        LogManager.StartLogging();
        LogControllerEntry(context);
        var executedAction = await next();
        LogControllerExit(executedAction);
        if (executedAction.Exception != null)
        {
            _root.Write();
            if (!LoggerConfiguration.IsSupressingExceptions)
                throw executedAction.Exception;
        }
        else if (!LoggerConfiguration.IsLoggingOnlyOnExceptions)
            _root.Write();
    }
    private void LogControllerEntry(ActionExecutingContext context)
    {
        (string className, string methodName) = GetNames(context);
        _root.LogEntry(
            new LogEntry(DateTime.Now,
                         className,
                         methodName,
                         context.ActionArguments));
    }
    private void LogControllerExit(ActionExecutedContext executedAction)
    {
        if (executedAction.Exception == null)
        {
            _root.LogExit(
                new LogExit(DateTime.Now, GetResult(executedAction)));
        }
        else
        {
            _root.LogExit(
                new LogExit(DateTime.Now, executedAction.Exception));
        }
    }
    private static object GetResult(ActionExecutedContext executedAction)
    {
        return executedAction.Result switch
        {
            ObjectResult objectResult => objectResult.Value,
            JsonResult jsonResult => jsonResult.Value,
            ContentResult contentResult => contentResult.Content,
            StatusCodeResult statusCodeResult => $"Status Code: {statusCodeResult.StatusCode}",
            RedirectResult redirectResult => $"Redirect to: {redirectResult.Url}",
            RedirectToActionResult redirectToActionResult => $"Redirect to Action: {redirectToActionResult.ActionName}",
            RedirectToRouteResult redirectToRouteResult => $"Redirect to Route: {redirectToRouteResult.RouteName}",
            _ => $"Case for `{executedAction.Result.GetType()}` result type was not implemented!"
        };
    }
    private static (string className, string methodName) GetNames(ActionExecutingContext context)
    {
        string displayName = context.ActionDescriptor.DisplayName;
        return (displayName[..displayName.LastIndexOf('.')],
                displayName[(displayName.LastIndexOf('.') + 1)..].Split(" ")[0]);
    }
}