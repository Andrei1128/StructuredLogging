using StructuredLogging.Data.BaseObjects;

namespace StructuredLogging.Data.ValueObjects
{
    public class LogObject : LogEntryBase
    {
        public object? Input { get; set; }
        public object? Output { get; set; }
    }
}
