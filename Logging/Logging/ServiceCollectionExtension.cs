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

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.HasMethodWithLogAttribute())
                    {
                        var interfaces = type.GetInterfaces();
                        foreach (var interf in interfaces)
                        {
                            services.AddScoped(interf, provider =>
                            {
                                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                                var logInterceptor = provider.GetRequiredService<LogInterceptor>();
                                var implementationInstance = ActivatorUtilities.CreateInstance(provider, type);
                                var proxy = proxyGenerator.CreateInterfaceProxyWithTarget(interf, implementationInstance, logInterceptor);
                                return proxy;
                            });
                        }
                    }
                }
            }

            return services;
        }
        private static bool HasMethodWithLogAttribute(this Type type)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(LogAttribute), false);

                if (attributes.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
