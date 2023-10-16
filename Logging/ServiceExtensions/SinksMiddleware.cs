using Logging.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Logging.ServiceExtensions;
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
public static partial class ServiceCollectionExtensions
{
    public static IApplicationBuilder InitializeCustomSinks(this IApplicationBuilder builder) => builder.UseMiddleware<SinksMiddleware>();
}
