using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests
{
    public class TestAboveRepository : ITestAboveRepository
    {
        private readonly ILogger _logger;
        public TestAboveRepository(ILogger logger)
        {
            _logger = logger;
        }

        public string Test(string data)
        {
            _logger.Information("Test_Above");
            var result = data;
            return "Test_Above";
        }
    }
    public interface ITestAboveRepository
    {
        public string Test(string data);
    }
}