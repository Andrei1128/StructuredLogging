using Castle.DynamicProxy;

namespace StructuredLogging.Proxy
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"Before executing {invocation.Method.Name}");

            invocation.Proceed();

            Console.WriteLine($"After executing {invocation.Method.Name}");
        }
    }
}
