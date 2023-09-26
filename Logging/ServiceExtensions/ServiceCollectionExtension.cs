using Castle.DynamicProxy;
using Logging.Configurations;
using Logging.Interceptors;
using Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogging<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
        {
            _ = lifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<TImplementation>(),
                ServiceLifetime.Transient => services.AddTransient<TImplementation>(),
                ServiceLifetime.Singleton => services.AddSingleton<TImplementation>(),
                _ => throw new ArgumentOutOfRangeException(nameof(lifetime), $"Unsupported service lifetime: {lifetime}")
            };

            services.Add(ServiceDescriptor.Describe(
                typeof(TService),
                provider =>
                {
                    var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                    var logInterceptor = provider.GetRequiredService<ILogger>();
                    var implementationInstance = provider.GetRequiredService<TImplementation>();
                    var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, logInterceptor);
                    return proxy;
                },
                lifetime
            ));
            return services;
        }
        public static LoggerConfiguration CreateLogger(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<ILogger, LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();

            return new LoggerConfiguration();
        }
    }
}