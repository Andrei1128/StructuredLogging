using Castle.DynamicProxy;
using Logging.Replaying.Objects;
using System.Diagnostics;

namespace RepeatableExecutionsTests;

public class MockInterceptor : IInterceptor
{
    private readonly IEnumerable<MockObject> _mocks;
    public MockInterceptor(IEnumerable<MockObject> mocks) => _mocks = mocks;

    [DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        foreach (var mock in _mocks)
        {
            if (mock.Method == invocation.Method.Name)
            {
                invocation.ReturnValue = "testalesss"; //mock.Output; 
            }
        }
    }
}
