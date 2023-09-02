using Castle.DynamicProxy;
using Logging.Attributes;
using Logging.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Logging
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStructuredLogging(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<LogInterceptor>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<StructuredLoggingAttribute>();

            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assembliesToScan)
            {
                var typesWithLogMethods = assembly.GetTypes()
                    .Where(type => type.GetMethods().Any(method => method.IsDefined(typeof(LogAttribute), true)));

                foreach (var type in typesWithLogMethods)
                {
                    var interceptor = services.BuildServiceProvider().GetRequiredService<LogInterceptor>();
                    var proxy = services.BuildServiceProvider().GetRequiredService<ProxyGenerator>().CreateClassProxy(type, interceptor);

                    services.AddScoped(type, serviceProvider => proxy);
                }
            }

            return services;
        }
    }
}
