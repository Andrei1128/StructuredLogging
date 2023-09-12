using Logging.Objects;
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
            LogEntry(context);
            var result = await next();
            LogExit(result);
            _logger.WriteToFile();
        }
        private void LogEntry(ActionExecutingContext context)
        {
            var names = GetNames(context);
            LogEntry entry = new LogEntry()
            {
                Time = DateTime.Now,
                Class = names.className,
                Method = names.methodName,
                Input = context.ActionArguments
            };
            _logger.LogEntry(entry);
        }
        private void LogExit(ActionExecutedContext result)
        {
            LogExit exit = new LogExit()
            {
                Time = DateTime.Now
            };
            if (result.Exception == null)
            {
                if (result.Result is ObjectResult objectResult)
                {
                    exit.Output = objectResult.Value;
                }
                else
                {
                    var resultType = result.Result?.GetType();
                    exit.Output = $"Case for \"{resultType}\" was not implemented!";
                }
            }
            else
            {
                exit.HasError = true;
                exit.Output = result.Exception;
            }
            _logger.LogExit(exit);
        }
        private (string className, string methodName) GetNames(ActionExecutingContext context)
        {
            string displayName = context.ActionDescriptor.DisplayName;
            return (displayName.Substring(0, displayName.LastIndexOf('.')),
                    displayName.Substring(displayName.LastIndexOf('.') + 1).Split(" ")[0]);
        }
    }
}