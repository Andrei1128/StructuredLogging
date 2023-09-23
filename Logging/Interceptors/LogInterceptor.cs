using Castle.DynamicProxy;
using Logging.Configurations;
using Logging.Manager;
using Logging.Objects;

namespace Logging.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
        private readonly ILog _root;
        private readonly Stack<Log> CallStack = new();
        private bool IsCallStackRoot = true;
        public LogInterceptor(ILog root) => _root = root;
        public void Intercept(IInvocation invocation)
        {
            if (!LogManager.IsLogging())
            {
                invocation.Proceed();
                return;
            }

            Log current = new()
            {
                Entry = new LogEntry(
                    DateTime.Now,
                    invocation.TargetType.FullName,
                    invocation.Method.Name,
                    invocation.Arguments)
            };
            if (!IsCallStackRoot)
            {
                Log parent = CallStack.Peek();
                parent.Interactions.Add(current);
            }
            else
            {
                _root.LogInteraction(current);
                IsCallStackRoot = false;
            }
            CallStack.Push(current);
            try
            {
                invocation.Proceed();
                current.Exit = new LogExit(DateTime.Now, invocation.ReturnValue);
            }
            catch (Exception ex)
            {
                current.Exit = new LogExit(DateTime.Now, ex);
                if (!LoggerConfiguration.IsSupressingExceptions)
                {
                    _root.WriteToFile();
                    throw;
                }
            }
            finally
            {
                CallStack.Pop();
            }
        }
    }
}