using Logging.Interceptors;
namespace RepeatableExecutionsTests
{
    public class TestRepository2 : ITestRepository2
    {
        private readonly ILogger2 _logger;
        public TestRepository2(ILogger2 logger)
        {
            _logger = logger;
        }
        public string Test(string data)
        {
            _logger.Information("testRepo2");
            var result = data;
            return result;
        }
    }
    public interface ITestRepository2
    {
        public string Test(string data);
    }
}