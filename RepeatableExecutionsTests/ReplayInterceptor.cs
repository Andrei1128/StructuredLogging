using Castle.DynamicProxy;

namespace RepeatableExecutionsTests;

public class ReplayInterceptor : IInterceptor
{
    private readonly string _method;
    private readonly object[]? _input;
    private readonly object? _output;
    public ReplayInterceptor(string method, object[] input, object output)
    {
        _method = method;
        _input = input;
        _output = output;
    }

    public void Intercept(Castle.DynamicProxy.IInvocation invocation)
    {
        if (invocation.Method.Name == _method)
        {
            invocation.ReturnValue = _output;
        }
        else
        {
            invocation.Proceed();
        }
    }
}
