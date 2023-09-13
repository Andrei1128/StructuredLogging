namespace Logging.Interceptors
{
    using Castle.DynamicProxy;
    using Logging.Objects;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class LogInterceptor : IInterceptor
    {
        private Log rootMethod = new Log { MethodName = "Main" };
        private Stack<Log> callStack = new Stack<Log>();

        public void Intercept(IInvocation invocation)
        {
            string methodName = invocation.Method.Name;
            Console.WriteLine(methodName);

            // Create a new method call node
            Log currentMethod = new Log { MethodName = methodName };

            // Add it to the parent's child calls (if not root)
            if (callStack.Count > 0)
            {
                Log parentMethod = callStack.Peek();
                parentMethod.ChildCalls.Add(currentMethod);
            }
            else
            {
                rootMethod.ChildCalls.Add(currentMethod);
            }

            // Push the current method onto the call stack
            callStack.Push(currentMethod);
            SerializeAndSave();
            try
            {
                // Proceed with the original method call
                invocation.Proceed();
            }
            finally
            {
                // Pop the current method from the call stack
                callStack.Pop();
            }
            SerializeAndSave();
        }
        public void SerializeAndSave()
        {
            string json = JsonConvert.SerializeObject(rootMethod, Formatting.Indented);
            Console.WriteLine(json);
            File.WriteAllText("method_calls.json", json);
            Console.WriteLine("Method call tree has been serialized to method_calls.json.");
        }
    }

}