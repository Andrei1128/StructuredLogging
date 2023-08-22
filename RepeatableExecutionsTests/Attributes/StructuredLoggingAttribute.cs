using Microsoft.AspNetCore.Mvc.Filters;
using RepeatableExecutionsTests.Helpers;

namespace RepeatableExecutionsTests.Attributes
{
    public class StructuredLoggingAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            CorrelationIdManager.Initialize();
            await next();
        }
    }
}
