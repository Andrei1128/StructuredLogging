using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logging
{
    public class StructuredLoggingAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ILog _logger;
        public StructuredLoggingAttribute(ILog logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Log(context, await next());
            _logger.WriteToFile();
        }
        private void Log(ActionExecutingContext context, ActionExecutedContext result)
        {
            var names = GetNames(context);
            LogObject entry = new LogObject()
            {
                Time = DateTime.Now,
                Class = names.className,
                Method = names.methodName,
                Input = context.ActionArguments
            };
            if (result.Exception == null)
            {
                if (result.Result is ObjectResult objectResult)
                {
                    entry.Output = objectResult.Value;
                }
                else
                {
                    var resultType = result.Result?.GetType();
                    entry.Output = $"Case for \"{resultType}\" was not implemented!";
                }
            }
            else
            {
                entry.HasError = true;
                entry.Output = result.Exception;
            }
            _logger.LogEntry(entry);
        }
        private (string className, string methodName) GetNames(ActionExecutingContext context)
        {
            string displayName = context.ActionDescriptor.DisplayName;
            return (displayName.Substring(0, displayName.LastIndexOf('.')),
                    displayName.Substring(displayName.LastIndexOf('.') + 1).Split(" ")[0]);
        }
    }
}