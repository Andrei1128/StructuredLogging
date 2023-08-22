using Castle.DynamicProxy;
using RepeatableExecutionsTests.Services;

namespace RepeatableExecutionsTests.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingInterceptor(this IServiceCollection services)
        {
            services.AddScoped<LogInterceptor>();
            services.AddSingleton(provider =>
            {
                return new ProxyGenerator();
            });

            services.AddScoped<IWeatherForecastService>(provider =>
            {
                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                var logInterceptor = provider.GetRequiredService<LogInterceptor>();

                var myService = new WeatherForecastService();
                var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<IWeatherForecastService>(myService, logInterceptor);
                return proxy;
            });

            return services;
        }
    }
}
