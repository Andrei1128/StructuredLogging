using Castle.DynamicProxy;
using RepeatableExecutionsTests.Attributes;
using RepeatableExecutionsTests.Helpers;
using System.Reflection;

namespace RepeatableExecutionsTests.Logging
{
    public class LogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (CorrelationIdManager.GetCurrentCorrelationId() != Guid.Empty)
            {
                var logAttribute = method.GetCustomAttribute<LogAttribute>();

                if (logAttribute != null)
                {
                    var arguments = invocation.Arguments;
                    logAttribute.LogBefore(arguments);
                }

                invocation.Proceed();

                if (logAttribute != null)
                {
                    var returnValue = invocation.ReturnValue;

                    logAttribute.LogAfter(returnValue);
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
