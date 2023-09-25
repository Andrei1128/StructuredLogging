namespace Logging.Configurations
{
    public class WriterConfigurations
    {
        private readonly LoggerConfiguration _config;
        public static string FilePath { get; private set; } = ".";
        public static string FileName { get; private set; } = $"log-{DateTime.Now:yyyyMMddHHmmssfffffff}";
        public static bool IsWritingToFile { get; private set; } = false;
        public WriterConfigurations(LoggerConfiguration config) => _config = config;
        public LoggerConfiguration SqlServer()
        {
            throw new NotImplementedException();
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
