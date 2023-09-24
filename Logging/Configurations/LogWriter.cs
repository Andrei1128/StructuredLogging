namespace Logging.Configurations
{
    public class LogWriter
    {
        private readonly LoggerConfiguration _config;
        public LogWriter(LoggerConfiguration config)
        {
            _config = config;
        }
        public LoggerConfiguration SqlServer()
        {
            throw new NotImplementedException();
            return _config;
        }
        public LoggerConfiguration File()
        {
            throw new NotImplementedException();
            return _config;
        }
    }
}
