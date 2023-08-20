using Microsoft.AspNetCore.Http;

namespace StructuredLogging.Middleware
{
    public class StructuredLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public StructuredLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}