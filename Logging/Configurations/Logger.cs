using Castle.DynamicProxy;
using Logging.Interceptors;
using Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Configurations
{
    public class Logger
    {
        public static bool IsSupressingExceptions { get; private set; } = false;
        public static bool IsLoggingOnlyOnExceptions { get; private set; } = false;
        public LogWriter WriteTo { get; }

        public Logger(IServiceCollection services)
        {
            services.AddSingleton<ProxyGenerator>();
            services.AddScoped<ILog, Log>();
            services.AddScoped<LogInterceptor>();
            services.AddScoped<StructuredLoggingAttribute>();
            WriteTo = new LogWriter();
        }

        public Logger SupressExceptions()
        {
            IsSupressingExceptions = true;
            return this;
        }

        public Logger LogOnlyOnExceptions()
        {
            IsLoggingOnlyOnExceptions = true;
            return this;
        }
    }
}
