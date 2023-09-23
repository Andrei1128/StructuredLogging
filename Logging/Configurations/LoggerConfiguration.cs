namespace Logging.Configurations
{
    public class LoggerConfiguration
    {
        public static bool IsSupressingExcetions { get; private set; } = false;
        public static bool IsLoggingOnlyOnExceptions { get; private set; } = false;
        public LogWriter WriteTo { get; }
        public LoggerConfiguration() => WriteTo = new LogWriter();
        public LoggerConfiguration SupressExceptions()
        {
            IsSupressingExcetions = true;
            return this;
        }
        public LoggerConfiguration LogOnlyOnExceptions()
        {
            IsLoggingOnlyOnExceptions = true;
            return this;
        }
    }
}
