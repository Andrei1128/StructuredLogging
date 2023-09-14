using Castle.DynamicProxy;
using Logging.Objects;

namespace Logging.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
        private Log Root = new Log(); //Get this somehow from StructuredLoggingAttribute
        private Stack<Log> CallStack = new Stack<Log>();
        private bool IsNotRoot = false;
        public void Intercept(IInvocation invocation)
        {
            Log current = new Log()
            {
                Entry = new LogEntry()
                {
                    Time = DateTime.Now,
                    Class = invocation.TargetType.FullName,
                    Method = invocation.Method.Name,
                    Input = invocation.Arguments,
                }
            };
            if (IsNotRoot)
            {
                Log parent = CallStack.Peek();
                parent.Interactions.Add(current);
            }
            else
            {
                Root.Interactions.Add(current);
                IsNotRoot = true;
            }
            CallStack.Push(current);
            try
            {
                invocation.Proceed();
                current.Exit = new LogExit()
                {
                    Time = DateTime.Now,
                    Output = invocation.ReturnValue,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                current.Exit = new LogExit()
                {
                    Time = DateTime.Now,
                    Output = ex,
                    HasError = true
                };
            }
            finally
            {
                CallStack.Pop();
            }
        }
    }
}