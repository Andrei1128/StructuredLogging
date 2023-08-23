using Castle.DynamicProxy;

namespace RepeatableExecutionsTests.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Decorate<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
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

            services.AddSingleton(provider =>
            {
                return new ProxyGenerator();
            });
            services.AddScoped<LogInterceptor>();

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
    }

}
