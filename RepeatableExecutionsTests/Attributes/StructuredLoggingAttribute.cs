using Microsoft.AspNetCore.Mvc.Filters;
using RepeatableExecutionsTests.Helpers;

namespace RepeatableExecutionsTests.Attributes
{
    public class StructuredLoggingAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            CorrelationIdManager.Initialize();
            Console.WriteLine(CorrelationIdManager.GetCurrentCorrelationId());
            Console.WriteLine("AsyncActionFilter: Before method call");
            await next();
            Console.WriteLine("AsyncActionFilter: After method call");
        }
    }
}
