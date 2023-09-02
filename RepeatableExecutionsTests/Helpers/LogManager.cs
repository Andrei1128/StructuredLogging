namespace Logging.Helpers
{
    public class LogManager
    {
        private static AsyncLocal<bool> IsLogging = new AsyncLocal<bool>();

        public static void StartLogging()
        {
            IsLogging.Value = true;
        }

        public static bool IsLoggingEnabled()
        {
            return IsLogging.Value;
        }
    }
}
