using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests
{
    public class TestRepository2 : ITestRepository2
    {
        private readonly ILogger _logger;
        public TestRepository2(ILogger logger)
        {
            _logger = logger;
        }
        public string Test(string data)
        {
            _logger.Information("Test_Repo2");
            var result = data;
            return "Test_Repo2";
        }
    }
    public interface ITestRepository2
    {
        public string Test(string data);
    }
}