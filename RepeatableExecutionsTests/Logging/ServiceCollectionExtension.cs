//using Castle.DynamicProxy;
//using Logging.Attributes;
//using Logging.Helpers;

//namespace Logging.Logging
//{
//    public static class ServiceCollectionExtension
//    {
//        public static IServiceCollection AddStructuredLogging(this IServiceCollection services)
//        {
//            services.AddSingleton<ProxyGenerator>();
//            services.AddScoped<ILog, Log>();
//            services.AddScoped<LogInterceptor>();
//            services.AddScoped<StructuredLoggingAttribute>();

//            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies();

//            foreach (var assembly in assembliesToScan)
//            {
//                var typesWithLogMethods = assembly.GetTypes()
//                    .Where(type => type.GetMethods().Any(method => method.IsDefined(typeof(LogAttribute), true)));

//                foreach (var type in typesWithLogMethods)
//                {
//                    var interceptor = services.BuildServiceProvider().GetRequiredService<LogInterceptor>();
//                    var proxy = services.BuildServiceProvider().GetRequiredService<ProxyGenerator>().CreateClassProxy(type, interceptor);

//                    services.AddScoped(type, serviceProvider => proxy);
//                }
//            }

//            return services;
//        }
//    }
//}
using Castle.DynamicProxy;
using Logging.Attributes;
using Logging.Helpers;

namespace Logging.Logging
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStructuredLogging(this IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();

            services.AddScoped(provider =>
            {
                var generator = provider.GetRequiredService<ProxyGenerator>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
                    var typesWithLogMethods = assembly.GetTypes()
                        .Where(type => type.GetMethods().Any(method => method.IsDefined(typeof(LogAttribute), true)));

                    foreach (var type in typesWithLogMethods)
                    {
                        var interceptor = provider.GetRequiredService<LogInterceptor>();
                        var proxy = generator.CreateClassProxy(type, interceptor);

                        services.AddScoped(type, serviceProvider => proxy);
                    }
                }

                return services;
            });

            return services;
        }
    }
}