namespace Logging
{
    public class LogObject
    {
        public DateTime Time { get; set; }
        public string Class { get; set; }
        public string Operation { get; set; }
        public object Input { get; set; }
        public object Output { get; set; }
    }
}
