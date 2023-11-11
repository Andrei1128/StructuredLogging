using Castle.DynamicProxy;
using Logging.Logging.Configurations;
using Logging.Logging.Interceptors;
using Logging.Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.ServiceExtensions;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggedScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Scoped);
    public static IServiceCollection AddLoggedTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Transient);
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
    public static LoggerConfiguration AddLogger(this IServiceCollection services)
    {
        services.AddSingleton<ProxyGenerator>();
        services.AddScoped<ILog, Log>();
        services.AddScoped<ILogger, LogInterceptor>();
        services.AddScoped<StructuredLoggingAttribute>();
        services.AddScoped<SinksMiddleware>();
        return new LoggerConfiguration(services);
    }
}