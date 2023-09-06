using Logging.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logging.Attributes
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
            LogManager.StartLogging();
            LogEntry(context);
            var resultContext = await next();
            LogExit(resultContext);
            _logger.WriteToFile();
        }
        private void LogEntry(ActionExecutingContext context)
        {
            var displayName = context.ActionDescriptor.DisplayName;
            LogObject entry = new LogObject()
            {
                Time = DateTime.Now,
                Class = displayName.Substring(0, displayName.LastIndexOf('.')),
                Operation = displayName.Substring(displayName.LastIndexOf('.') + 1).Split(" ")[0],
                Input = context.ActionArguments
            };
            _logger.LogEntry(entry);
        }
        private void LogExit(ActionExecutedContext resultContext)
        {
            LogObject exit = new LogObject();
            exit.Time = DateTime.Now;
            if (resultContext.Exception == null)
            {
                if (resultContext.Result is ObjectResult objectResult)
                {
                    exit.Output = objectResult.Value;
                }
                else
                {
                    var resultType = resultContext.Result?.GetType();
                    exit.Output = $"Case for \"{resultType}\" was not implemented!";
                }
            }
            else
            {
                exit.Output = resultContext.Exception;
            }
            _logger.LogExit(exit);
        }
    }
}
