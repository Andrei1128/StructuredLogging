using RepeatableExecutions.Data.BaseObjects;

namespace RepeatableExecutions.Data.ValueObjects
{
    public class LogObject : LogEntryBase
    {
        public object? Input { get; set; }
        public object? Output { get; set; }
    }
}
