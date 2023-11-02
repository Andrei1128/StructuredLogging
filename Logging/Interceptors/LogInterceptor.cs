using Castle.DynamicProxy;
using Logging.Configurations;
using Logging.Manager;
using Logging.Objects;

namespace Logging.Interceptors;
public class LogInterceptor : ILogger
{
    private readonly ILog _root;
    private readonly Stack<Log> CallStack = new();
    private bool IsCallStackRoot = true;
    private Log? Current = null;
    public LogInterceptor(ILog root) => _root = root;
    public void Intercept(Castle.DynamicProxy.IInvocation invocation)
    {
        if (!LogManager.IsLogging)
        {
            invocation.Proceed();
            return;
        }
        Current = new Log()
        {
            Entry = new LogEntry(
                DateTime.Now,
                string.Join(",", invocation.TargetType.AssemblyQualifiedName.Split(',').Take(2)),
                invocation.Method.Name,
                invocation.Arguments)
        };
        if (!IsCallStackRoot)
        {
            if (CallStack.TryPeek(out Log? parent))
                parent.Interactions.Add(Current);
            else
                _root.AddInteraction(Current);
        }
        else
        {
            _root.AddInteraction(Current);
            IsCallStackRoot = false;
        }
        CallStack.Push(Current);
        try
        {
            invocation.Proceed();
            Current = CallStack.Pop();
            Current.Exit = new LogExit(DateTime.Now, invocation.ReturnValue);
            if (CallStack.TryPeek(out Log? current))
                Current = current;
        }
        catch (Exception ex)
        {
            Current = CallStack.Pop();
            Current.Exit = new LogExit(DateTime.Now, ex);
            if (!LoggerConfiguration.IsSupressingExceptions)
            {
                _root.Write();
                throw;
            }
            else if (CallStack.TryPeek(out Log? current))
                Current = current;
        }
    }
    //public TOutput LogMethod<TOutput>(Func<object[], TOutput> action, params object[] inputs)
    //{
    //    if (!LogManager.IsLogging)
    //        return action.Invoke(inputs);
    //    TOutput output = default;
    //    var stackFrame = new StackFrame(2);
    //    MethodBase method = stackFrame.GetMethod();
    //    Current = new Log()
    //    {
    //        Entry = new LogEntry(
    //            DateTime.Now,
    //            method.ReflectedType.FullName,
    //            method.Name,
    //            inputs)
    //    };
    //    if (!IsCallStackRoot)
    //    {
    //        if (CallStack.TryPeek(out Log? parent))
    //            parent.Interactions.Add(Current);
    //        else
    //            _root.AddInteraction(Current);
    //    }
    //    else
    //    {
    //        _root.AddInteraction(Current);
    //        IsCallStackRoot = false;
    //    }
    //    CallStack.Push(Current);
    //    try
    //    {
    //        output = action.Invoke(inputs);
    //        Current = CallStack.Pop();
    //        Current.Exit = new LogExit(DateTime.Now, output);
    //        if (CallStack.TryPeek(out Log? current))
    //            Current = current;
    //    }
    //    catch (Exception ex)
    //    {
    //        Current = CallStack.Pop();
    //        Current.Exit = new LogExit(DateTime.Now, ex);
    //        if (!LoggerConfiguration.IsSupressingExceptions)
    //        {
    //            _root.Write();
    //            throw;
    //        }
    //        else if (CallStack.TryPeek(out Log? current))
    //            Current = current;
    //    }
    //    return output;
    //}
    private void Log(string content, LogLevels level)
    {
        string log = $"[{level}] {content}";
        if (Current == null || CallStack.Count == 0)
            _root.AddInformation(log);
        else
            Current.Infos.Add(log);
    }
    public void Verbose(string content) => Log(content, LogLevels.Verbose);
    public void Debug(string content) => Log(content, LogLevels.Debug);
    public void Information(string content) => Log(content, LogLevels.Information);
    public void Warning(string content) => Log(content, LogLevels.Warning);
    public void Error(string content) => Log(content, LogLevels.Error);
    public void Fatal(string content) => Log(content, LogLevels.Fatal);
}
public interface ILogger : IInterceptor
{
    //public TOutput LogMethod<TOutput>(Func<object[], TOutput> action, params object[] inputs);
    public void Verbose(string content);
    public void Debug(string content);
    public void Information(string content);
    public void Warning(string content);
    public void Error(string content);
    public void Fatal(string content);
}