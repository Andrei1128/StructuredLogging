namespace Logging.Replaying.Objects;

public class MockObject
{
    public string Method { get; }
    public object? Output { get; }
    public MockObject(string method, object? output)
    {
        Method = method;
        Output = output;
    }
}
