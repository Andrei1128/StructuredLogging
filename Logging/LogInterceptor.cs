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
                Method = invocation.Method.Name,
                Input = invocation.Arguments,
            };
            try
            {
                invocation.Proceed();
                interaction.Output = invocation.ReturnValue;
            }
            catch (Exception ex)
            {
                interaction.HasError = true;
                interaction.Output = ex;
            }
            Log log = new Log()
            {
                Entry = interaction
            };
            _logger.LogInteraction(log);
        }
    }
}