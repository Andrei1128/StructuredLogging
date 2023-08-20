using Microsoft.AspNetCore.Builder;
namespace StructuredLogging.Middleware
{
    public static class StructuredLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseStructuredLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StructuredLoggingMiddleware>();
        }
    }
}
