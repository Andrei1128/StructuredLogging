namespace Logging.Objects
{
    public class LogEntry
    {
        public DateTime Time { get; }
        public string Class { get; }
        public string Method { get; }
        public object Input { get; }
        public LogEntry(DateTime time, string @class, string method, object input)
        {
            Time = time;
            Class = @class;
            Method = method;
            Input = input;
        }
    }
}