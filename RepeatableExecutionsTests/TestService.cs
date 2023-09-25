using Logging.Interceptors;

namespace RepeatableExecutionsTests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _repository;
        private readonly ITestRepository2 _repository2;
        private readonly ILogger2 _logger;
        public TestService(ITestRepository repository, ITestRepository2 repository2, ILogger2 logger)
        {
            _repository = repository;
            _repository2 = repository2;
            _logger = logger;
        }
        public string Test(string data)
        {
            var result = _repository.Test(data + "_Repository");
            _logger.Information("testingtest");
            var result2 = _repository2.Test(data + "_Repository2");
            return result + "_" + result2;
        }
    }
    public interface ITestService
    {
        public string Test(string data);
    }
}