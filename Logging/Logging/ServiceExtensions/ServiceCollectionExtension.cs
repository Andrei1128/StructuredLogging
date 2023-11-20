using Castle.DynamicProxy;
using Logging.Logging.Configurations;
using Logging.Logging.Interceptors;
using Logging.Logging.Objects;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Reflection.Emit;

namespace Logging.ServiceExtensions;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggedScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    => services.AddLoggedService<TService, TImplementation>(ServiceLifetime.Scoped);
    public static IServiceCollection AddLoggedScoped<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    => services.AddLoggedService<TImplementation>(ServiceLifetime.Scoped);
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
                TService? proxy = null;
                if (typeof(TService).IsInterface)
                {
                    proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, logInterceptor);
                    return proxy;
                }
                else
                {
                    Type testType = typeof(TImplementation);
                    Type dynamicType = VirtualizeClassMethods(testType);
                    var proxy2 = proxyGenerator.CreateClassProxy(dynamicType, logInterceptor);
                    return proxy2;
                }
            },
            lifetime
        ));
        return services;
    }
    private static IServiceCollection AddLoggedService<TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TImplementation : class
    {
        var serviceProvide = services.BuildServiceProvider();
        var proxyGenerator = serviceProvide.GetRequiredService<ProxyGenerator>();
        var logInterceptor = serviceProvide.GetRequiredService<ILogger>();
        Type testType = typeof(TImplementation);
        Type dynamicType = VirtualizeClassMethods(testType);
        var proxy2 = proxyGenerator.CreateClassProxy(dynamicType, logInterceptor);
        services.Add(new ServiceDescriptor(testType, proxy2.GetType(), lifetime));
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

    private static int uniqueId = 0;
    private static Type VirtualizeClassMethods(Type baseType)
    {
        var assemblyName = new AssemblyName("DynamicAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
        string derivedClassName = $"DynamicClass{uniqueId++}_{baseType.Name}";

        var typeBuilder = moduleBuilder.DefineType(derivedClassName, TypeAttributes.Public | TypeAttributes.Class, baseType);

        foreach (var methodInfo in baseType.GetMethods())
        {
            var methodBuilder = typeBuilder.DefineMethod(
                methodInfo.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                methodInfo.ReturnType,
                Array.ConvertAll(methodInfo.GetParameters(), p => p.ParameterType)
            );

            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);

            for (int i = 1; i <= methodInfo.GetParameters().Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_S, i);
            }

            ilGenerator.Emit(OpCodes.Call, methodInfo);
            ilGenerator.Emit(OpCodes.Ret);
        }

        return typeBuilder.CreateType();
    }
}