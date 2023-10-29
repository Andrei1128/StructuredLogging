using Logging.Objects;
using Newtonsoft.Json;

namespace RepeatableExecutionsTests;
public class CustomSink : IObserver
{
    public CustomSink(ILog log) => log.Attach(this);
    public void Write(ILog subject)
    {
        string serializedLog = JsonConvert.SerializeObject(subject);
        //Console.WriteLine(serializedLog);
    }
}
