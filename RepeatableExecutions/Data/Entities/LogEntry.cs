using RepeatableExecutions.Data.BaseObjects;
using RepeatableExecutions.Data.ValueObjects;

namespace RepeatableExecutions.Data.Entities
{
    public class LogEntry : LogEntryBase
    {
        public LogObject Entry { get; set; } = new LogObject();
        public List<LogObject> Interactions { get; set; } = new List<LogObject>();
        public LogObject Exit { get; set; } = new LogObject();
    }
}
