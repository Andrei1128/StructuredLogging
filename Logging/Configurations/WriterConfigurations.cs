using Microsoft.Extensions.DependencyInjection;

namespace Logging.Configurations
{
    public class WriterConfigurations
    {
        private readonly LoggerConfiguration _config;
        public static string FilePath { get; private set; } = ".";
        public static string FileName { get; private set; } = $"log-{DateTime.Now:yyyyMMddHHmmssfffffff}";
        public static bool IsWritingToFile { get; private set; } = false;
        private readonly IServiceCollection _serviceCollection;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Type> CustomSinks = new();
        public WriterConfigurations(LoggerConfiguration config, IServiceCollection serviceCollection)
        {
            _config = config;
            _serviceCollection = serviceCollection;
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        public static void RegisterCustomSinks()
        {
        }
        public LoggerConfiguration CustomWriter<TSink>() where TSink : class
        {
            CustomSinks.Add(typeof(TSink));
            _serviceCollection.AddScoped<TSink>();
            return _config;
        }
        public LoggerConfiguration File(string filePath = ".")
        {
            IsWritingToFile = true;
            FilePath = filePath;
            return _config;
        }
    }
}
