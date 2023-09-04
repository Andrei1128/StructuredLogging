﻿using Castle.DynamicProxy;
using Logging.Attributes;
using Logging.Helpers;
using System.Reflection;

namespace Logging.Logging
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
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var logAttribute = method.GetCustomAttribute<LogAttribute>();
            if (logAttribute == null || !LogManager.IsLoggingEnabled())
            {
                invocation.Proceed();
            }
            else
            {
                LogObject interaction = new LogObject();
                // Before execution
                interaction.Time = DateTime.Now;
                interaction.Operation = method.Name;
                interaction.Input = invocation.Arguments;

                try
                {
                    // Execution
                    invocation.Proceed();

                    // After execution
                    interaction.Output = invocation.ReturnValue;
                }
                catch (Exception ex)
                {
                    // Error handling
                    interaction.Output = ex;
                }
                _logger.LogInteraction(interaction);
            }
        }
    }
}