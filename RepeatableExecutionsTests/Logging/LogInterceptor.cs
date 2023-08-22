using Castle.DynamicProxy;
using RepeatableExecutionsTests.Attributes;
using System.Reflection;

namespace RepeatableExecutionsTests.Logging
{
    public class LogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            var logAttribute = method.GetCustomAttribute<LogAttribute>();

            if (logAttribute != null)
            {
                var methodName = $"{method.DeclaringType.Name}.{method.Name}";
                logAttribute.LogBefore(methodName);
            }

            invocation.Proceed();

            if (logAttribute != null)
            {
                var methodName = $"{method.DeclaringType.Name}.{method.Name}";
                logAttribute.LogAfter(methodName);
            }
        }
    }
}
