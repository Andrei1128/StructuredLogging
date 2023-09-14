namespace Logging.Objects
{
    public class LogExit
    {
        public DateTime Time { get; set; }
        public object Output { get; set; }
        public bool HasError { get; set; }
    }
}