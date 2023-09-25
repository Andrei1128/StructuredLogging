using Logging.Interceptors;

namespace RepeatableExecutionsTests
{
    public class TestAboveRepository : ITestAboveRepository
    {
        private readonly ILogger2 _logger;
        public TestAboveRepository(ILogger2 logger)
        {
            _logger = logger;
        }

        public string Test(string data)
        {
            _logger.Information("testAbove");
            var result = data;
            return result;
        }
    }
    public interface ITestAboveRepository
    {
        public string Test(string data);
    }
}