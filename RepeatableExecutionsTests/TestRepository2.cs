using ILogger = Logging.Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests;
public class TestRepository2 : ITestRepository2
{
    private readonly ILogger _logger;
    public TestRepository2(ILogger logger)
    {
        _logger = logger;
    }
    public void Test(string data, out string result)
    {
        result = "Test_Repo2";
    }
}
public interface ITestRepository2
{
    public void Test(string data, out string result);
}