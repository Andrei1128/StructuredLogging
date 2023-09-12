using Castle.DynamicProxy;
using Logging.Objects;

namespace Logging.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
        private readonly ILog _logger;
        public LogInterceptor(ILog logger)
        {
            _logger = logger;
        }
        public void Intercept(IInvocation invocation)
        {
            LogEntry entry = new LogEntry()
            {
                Time = DateTime.Now,
                Class = invocation.TargetType.FullName,
                Method = invocation.Method.Name,
                Input = invocation.Arguments,
            };
            Log interaction = new Log(entry);
            _logger.LogInteraction(interaction);
            LogExit exit = new LogExit();
            try
            {
                invocation.Proceed();
                exit.Time = DateTime.Now;
                exit.Output = invocation.ReturnValue;
            }
            catch (Exception ex)
            {
                exit.Time = DateTime.Now;
                exit.HasError = true;
                exit.Output = ex;
            }
            finally
            {
                _logger.LogExit(exit);
            }
        }
    }
}