using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RepeatableExecutionsTests.Helpers;
using System.Text.Json;

namespace RepeatableExecutionsTests.Attributes
{
    public class StructuredLoggingAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            CorrelationIdManager.Initialize();

            // Before Execution
            string payload = JsonSerializer.Serialize(context.ActionArguments);
            Console.WriteLine(payload);

            // Execution
            var resultContext = await next();

            // After Execution
            if (resultContext.Exception == null)
            {
                if (resultContext.Result is ObjectResult objectResult)
                {
                    string response = JsonSerializer.Serialize(objectResult.Value);
                    Console.WriteLine(response);
                }
                else if (resultContext.Result is ContentResult contentResult) ;
                //{
                //    Console.WriteLine(contentResult.Content);
                //}
            }
            else
            {
                Console.WriteLine(resultContext.Exception);
            }

        }
    }
}
