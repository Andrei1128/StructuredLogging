using Castle.DynamicProxy;
using Logging.Attributes;
using Logging.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStructuredLogging(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();

            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assembliesToScan)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var interfaces = type.GetInterfaces();
                    foreach (var @interface in interfaces)
                    {
                        if (@interface.IsInterface && @interface != typeof(IInterceptor))
                        {
                            if (HasMethodWithLogAttribute(type))
                            {
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
