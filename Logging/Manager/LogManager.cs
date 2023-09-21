namespace Logging.Manager
{
    public class LogManager
    {
        private static bool isLogging = false;
        public static void StartLogging() => isLogging = true;
        public static bool IsLogging() => isLogging;
    }
}
