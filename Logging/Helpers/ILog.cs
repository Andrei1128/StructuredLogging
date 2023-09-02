namespace Logging.Helpers
{
    public interface ILog
    {
        public void LogEntry(LogObject entry);
        public void LogInteraction(LogObject interaction);
        public void LogExit(LogObject exit);
        public void WriteToFile();
    }
}
