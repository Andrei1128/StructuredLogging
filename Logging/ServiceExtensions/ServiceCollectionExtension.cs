using Castle.DynamicProxy;
using Logging.Configurations;
using Logging.Interceptors;
using Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggedScoped<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Scoped);
        public static IServiceCollection AddLoggedTransient<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Transient);
        //public static IServiceCollection AddLoggedSingleton<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Singleton);
        private static IServiceCollection AddLoggedService<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
        {
            services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), lifetime));
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
        public static LoggerConfiguration RegisterLogger(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<ILogger, LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();
            return new LoggerConfiguration(services.BuildServiceProvider());
        }
    }
}