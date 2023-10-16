using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests;
public class TestRepository2 : ITestRepository2
{
    private readonly ITestAboveRepository _aboveRepository;
    private readonly ILogger _logger;
    public TestRepository2(ILogger logger, ITestAboveRepository aboveRepository)
    {
        _logger = logger;
        _aboveRepository = aboveRepository;
    }
    public string Test(string data)
    {
        _logger.Information("Test_Repo2");
        var result = _aboveRepository.Test(data + "_AboveRepository");
        _logger.Warning("Test_Repo2");
        return "Test_Repo2";
    }
}
public interface ITestRepository2
{
    public string Test(string data);
}