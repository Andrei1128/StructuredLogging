using Castle.DynamicProxy;
using Logging.Objects;
using System.Text.Json;

namespace Logging.Interceptors
{

    public class LogInterceptor : IInterceptor
    {
        private Log Root = new Log();
        private Stack<Log> CallStack = new Stack<Log>();
        private bool IsNotRoot = false;

        public void Intercept(IInvocation invocation)
        {
            string methodName = invocation.Method.Name;
            Console.WriteLine(methodName);

            // Create a new method call node
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

            // Add it to the parent's child calls (if not Root)
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

            // Push the current method onto the call stack
            CallStack.Push(current);
            try
            {
                // Proceed with the original method call
                invocation.Proceed();
                current.Exit = new LogExit()
                {
                    Time = DateTime.Now,
                    Output = invocation.ReturnValue
                };
            }
            catch (Exception ex)
            {
                current.Exit = new LogExit()
                {
                    Time = DateTime.Now,
                    HasError = true,
                    Output = ex
                };

            }
            finally
            {
                // Pop the current method from the call stack
                CallStack.Pop();
                if (CallStack.Count == 0 && IsNotRoot)
                    WriteToFile();
            }
        }
        public void WriteToFile()
        {
            string folderPath = "../Logging/logs";
            string serializedLog = JsonSerializer.Serialize(Root);
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            File.WriteAllText($"{folderPath}/{guid}.json", serializedLog);
        }
    }
}