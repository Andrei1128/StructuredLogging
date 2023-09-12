using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogging<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
        {
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    services.AddTransient<TImplementation>();
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TImplementation>();
                    break;
                case ServiceLifetime.Scoped:
                default:
                    services.AddScoped<TImplementation>();
                    break;
            }
            services.Add(ServiceDescriptor.Describe(
                typeof(TService),
                provider =>
                {
                    var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                    var logInterceptor = provider.GetRequiredService<LogInterceptor>();
                    var implementationInstance = provider.GetRequiredService<TImplementation>();
                    var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, logInterceptor);
                    return proxy;
                },
                lifetime
            ));

            return services;
        }
        public static IServiceCollection InitializeLogging(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();
            return services;
        }
    }

}