namespace Logging.Objects
{
    public class LogEntry
    {
        public DateTime Time { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public object Input { get; set; }
    }
}