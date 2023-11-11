using Logging.Logging.Objects;

namespace RepeatableExecutionsTests;
public class CustomSink : Observer
{
    public CustomSink(ILog log) : base(log) { }

    public override async Task Write(string subject)
    {
        await Console.Out.WriteLineAsync(subject);
    }
}
