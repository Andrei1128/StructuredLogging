namespace RepeatableExecutionsTests;
public class TestAboveRepository : ITestAboveRepository
{
    //private readonly ILogger _logger;
    //public TestAboveRepository(ILogger logger)
    //{
    //    _logger = logger;
    //}
    //public string LoggedTest(string message, int number) => _logger.LogMethod(Test, message, number);
    public string Test(string data, int number)
    {
        return "AboveTest";
    }
}
public interface ITestAboveRepository
{
    //public string LoggedTest(string message, int number);
    public string Test(string data, int number);
}