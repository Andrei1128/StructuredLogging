using Castle.DynamicProxy;
using Logging.Attributes;
using Logging.Helpers;
using Logging.Logging;
using RepeatableExecutionsTests.Services;

namespace RepeatableExecutionsTests.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStructuredLogging(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();

            // Get the assemblies you want to scan for services to intercept.
            var assembliesToScan = new[] { typeof(WeatherForecastService).Assembly };

            foreach (var assembly in assembliesToScan)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    var interfaces = type.GetInterfaces();

                    foreach (var @interface in interfaces)
                    {
                        // Check if the service is an interface and not the interceptor interface itself.
                        if (@interface.IsInterface && @interface != typeof(IInterceptor))
                        {
                            // Check if the class has methods with LogAttribute.
                            if (HasMethodWithLogAttribute(type))
                            {
                                // Register the service with the interceptor.
                                services.AddScoped(@interface, provider =>
                                {
                                    var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                                    var logInterceptor = provider.GetRequiredService<LogInterceptor>();
                                    var implementationInstance = ActivatorUtilities.CreateInstance(provider, type);

                                    var proxy = proxyGenerator.CreateInterfaceProxyWithTarget(@interface, implementationInstance, logInterceptor);
                                    return proxy;
                                });
                            }
                        }
                    }
                }
            }

            return services;
        }

        private static bool HasMethodWithLogAttribute(Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(LogAttribute), true);

                if (attributes.Length > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
