using System.Text.Json;

namespace RepeatableExecutionsTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class LogAttribute : Attribute
    {
        public void LogBefore(object[] arguments)
        {
            string serializedInput = JsonSerializer.Serialize(arguments);
            Console.WriteLine($"Input: {serializedInput}");
        }

        public void LogAfter(object returnValue)
        {
            string serializedOutput = JsonSerializer.Serialize(returnValue);
            Console.WriteLine($"Output: {serializedOutput}");
        }
    }
}
