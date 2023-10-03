using Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Configurations
{
    public class WriterConfigurations
    {
        private readonly LoggerConfiguration _config;
        public static string FilePath { get; private set; } = ".";
        public static string FileName { get; private set; } = $"log-{DateTime.Now:yyyyMMddHHmmssfffffff}";
        public static bool IsWritingToFile { get; private set; } = false;
        private IServiceProvider _serviceProvider;
        public WriterConfigurations(LoggerConfiguration config, IServiceProvider serviceProvider)
        {
            _config = config;
            _serviceProvider = serviceProvider;
        }
        public LoggerConfiguration CustomWriter(Type writerType)
        {
            var log = _serviceProvider.GetRequiredService<ILog>();
            Activator.CreateInstance(writerType, new object[] { log });
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
