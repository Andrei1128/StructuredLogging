using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests
{
    public class TestRepository
    //: ITestRepository
    {
        private readonly ITestAboveRepository _aboveRepository;
        private readonly ILogger _logger;
        public TestRepository(ITestAboveRepository aboveRepository, ILogger logger)
        {
            _aboveRepository = aboveRepository;
            _logger = logger;
        }
        public string Test(string data)
        {
            _logger.Information("Test_Repo");
            var result = _aboveRepository.Test(data + "_AboveRepository");
            _logger.Information("Test_Repo");
            return "Test_Repo";
        }
    }
    public interface ITestRepository
    {
        public string Test(string data);
    }
}