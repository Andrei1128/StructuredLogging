using Castle.DynamicProxy;
using Logging.Configurations;
using Logging.Interceptors;
using Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        #region Service Registers
        public static IServiceCollection AddLoggedScoped<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Scoped);
        public static IServiceCollection AddLoggedTransient<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Transient);
        public static IServiceCollection AddLoggedSingleton<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Singleton);
        public static IServiceCollection AddLoggedScoped<TImplementation>(this IServiceCollection services) where TImplementation : class => services.AddLoggedService<TImplementation>(ServiceLifetime.Scoped);
        public static IServiceCollection AddLoggedTransient<TImplementation>(this IServiceCollection services) where TImplementation : class => services.AddLoggedService<TImplementation>(ServiceLifetime.Transient);
        public static IServiceCollection AddLoggedSingleton<TImplementation>(this IServiceCollection services) where TImplementation : class => services.AddLoggedService<TImplementation>(ServiceLifetime.Singleton);
        #endregion
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
        private static IServiceCollection AddLoggedService<TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TImplementation : class
        {
            services.Add(ServiceDescriptor.Describe(
                typeof(TImplementation),
                provider =>
                {
                    var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                    var logInterceptor = provider.GetRequiredService<ILogger>();
                    // provide somehow a instance of the implementation
                    var implementationInstance = provider.GetRequiredService<TImplementation>();
                    var proxy = proxyGenerator.CreateClassProxyWithTarget(implementationInstance, logInterceptor);
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

            return new LoggerConfiguration();
        }
    }
}