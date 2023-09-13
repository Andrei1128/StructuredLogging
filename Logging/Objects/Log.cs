namespace Logging.Objects
{
    public class Log
    {
        public string MethodName { get; set; }
        public List<Log> ChildCalls { get; set; } = new List<Log>();
    }
}