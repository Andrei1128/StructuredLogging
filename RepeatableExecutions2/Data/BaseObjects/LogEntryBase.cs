namespace StructuredLogging.Data.BaseObjects
{
    public class LogEntryBase
    {
        public DateTimeOffset Time { get; set; }
        public string Operation { get; set; } = string.Empty;
    }
}
