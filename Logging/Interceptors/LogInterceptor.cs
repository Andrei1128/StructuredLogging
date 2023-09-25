﻿using Castle.DynamicProxy;
using Logging.Configurations;
using Logging.Manager;
using Logging.Objects;

namespace Logging.Interceptors
{
    public class LogInterceptor : ILogger2
    {
        private readonly ILog _root;
        private readonly Stack<Log> CallStack = new();
        private bool IsCallStackRoot = true;
        private Log Current;
        public LogInterceptor(ILog root) => _root = root;
        public void Information(string info)
        {
            if (Current == null || CallStack.Count == 0)
                _root.Information(info);
            else
                Current.Infos.Add(info);
        }
        public void Intercept(IInvocation invocation)
        {
            if (!LogManager.IsLogging)
            {
                invocation.Proceed();
                return;
            }
            Current = new()
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
                parent.Interactions.Add(Current);
            }
            else
            {
                _root.LogInteraction(Current);
                IsCallStackRoot = false;
            }
            CallStack.Push(Current);
            try
            {
                invocation.Proceed();
                Current.Exit = new LogExit(DateTime.Now, invocation.ReturnValue);
            }
            catch (Exception ex)
            {
                Current.Exit = new LogExit(DateTime.Now, ex);
                if (!LoggerConfiguration.IsSupressingExceptions)
                {
                    _root.Write();
                    throw;
                }
            }
            finally
            {
                CallStack.Pop();
            }
        }
    }
    public interface ILogger2 : IInterceptor
    {
        public void Information(string info);
    }
}