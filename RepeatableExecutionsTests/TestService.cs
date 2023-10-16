using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests;
public class TestService : ITestService
{
    private readonly ITestRepository _repository;
    private readonly ITestRepository2 _repository2;
    private readonly ILogger _logger;
    public TestService(
        ITestRepository repository,
        ITestRepository2 repository2, ILogger logger)
    {
        _repository = repository;
        _repository2 = repository2;
        _logger = logger;
    }
    public string Test(string data)
    {
        _logger.Information("Test_Service");
        var result = _repository.Test(data + "_Repository");
        _logger.Information("Test_Service");
        var result2 = _repository2.Test(data + "_Repository2");
        _logger.Information("Test_Service");
        return "Test_Service";
    }
}
public interface ITestService
{
    public string Test(string data);
}