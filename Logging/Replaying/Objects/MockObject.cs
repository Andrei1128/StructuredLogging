using System.Diagnostics;

namespace Logging.Replaying.Objects;

public class MockObject
{
    public int Id { get; }
    public string Method { get; }
    public object? Output { get; }
    [DebuggerStepThrough]
    public MockObject(int id, string method, object? output)
    {
        Id = id;
        Method = method;
        Output = output;
    }
}
