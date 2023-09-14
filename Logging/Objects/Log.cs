namespace Logging.Objects
{
    public class Log
    {
        public LogEntry Entry { get; set; }
        public LogExit Exit { get; set; }
        public List<Log> Interactions { get; set; } = new List<Log>();
    }
}