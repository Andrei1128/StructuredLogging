using Castle.DynamicProxy;
using Logging.Objects;

namespace Logging.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
        private readonly ILog _root;
        private Stack<Log> CallStack = new Stack<Log>();
        private bool IsNotCallStackRoot = false;
        public LogInterceptor(ILog root)
        {
            _root = root;
        }
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
            if (IsNotCallStackRoot)
            {
                Log parent = CallStack.Peek();
                parent.Interactions.Add(current);
            }
            else
            {
                _root.LogInteraction(current);
                IsNotCallStackRoot = true;
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