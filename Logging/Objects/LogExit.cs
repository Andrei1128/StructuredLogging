namespace Logging.Objects;
public class LogExit
{
    public DateTime Time { get; }
    public object Output { get; }
    public LogExit(DateTime time, object output)
    {
        Time = time;
        Output = output;
    }
}