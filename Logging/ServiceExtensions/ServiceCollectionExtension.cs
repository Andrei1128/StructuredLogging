using Castle.DynamicProxy;
using Logging.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogging<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped<TImplementation>();
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient<TImplementation>();
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TImplementation>();
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
            services.AddScoped<LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();
            return services;
        }
    }
}