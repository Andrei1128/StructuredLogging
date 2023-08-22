namespace RepeatableExecutionsTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class LogAttribute : Attribute
    {
        public void LogBefore(string methodName)
        {
            Console.WriteLine($"Before executing {methodName}.");
        }

        public void LogAfter(string methodName)
        {
            Console.WriteLine($"After executing {methodName}.");
        }
    }
}
