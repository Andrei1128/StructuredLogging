
using Logging.Configurations;

namespace RepeatableExecutionsTests
{
    public class SinksMiddleware : IMiddleware
    {
        public SinksMiddleware(IServiceProvider serviceProvider)
        {
            var types = WriterConfigurations.GetRegisteredTypes();
            foreach (var type in types)
                serviceProvider.GetService(type);
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) => await next(context);
    }
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder CreateSinks(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SinksMiddleware>();
        }
    }
}
