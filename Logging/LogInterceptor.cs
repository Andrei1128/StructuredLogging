using Castle.DynamicProxy;

namespace Logging
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
            //CREATE A BUILDER FOR LOG
            LogObject log;
            try
            {
                log = CreateInteraction(invocation);
                invocation.Proceed();
                log.Output = invocation.ReturnValue;
            }
            catch (Exception ex)
            {
                log.Output = ex.ToString();
            }
            finally
            {
                _logger.AddInteraction(interaction);
            }
        }
        private LogObject CreateInteraction(IInvocation invocation)
        {
            return new LogObject()
            {
                Time = DateTime.Now,
                Class = invocation.TargetType.FullName,
                Operation = invocation.Method.Name,
                Input = invocation.Arguments,
            };
        }
    }
}
