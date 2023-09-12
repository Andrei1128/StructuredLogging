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
            LogObject interaction = new LogObject()
            {
                Time = DateTime.Now,
                Class = invocation.TargetType.FullName,
                Operation = invocation.Method.Name,
                Input = invocation.Arguments,
            };
            invocation.Proceed();
            try
            {
                invocation.Proceed();
                interaction.Output = invocation.ReturnValue;
            }
            catch (Exception ex)
            {
                interaction.Output = ex;
            }
            _logger.LogInteraction(interaction);
        }
    }
}