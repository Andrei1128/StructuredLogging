namespace RepeatableExecutionsTests;
public class TestRepository : ITestRepository
{
    private readonly ITestAboveRepository _aboveRepository;
    //private readonly ILogger _logger;
    public TestRepository(ITestAboveRepository aboveRepository
        //, ILogger logger
        )
    {
        _aboveRepository = aboveRepository;
        //_logger = logger;
    }
    public (string, TestObject) Test(string data, TestObject obj)
    {
        var result = _aboveRepository.Test(data + "_AboveRepository", 1);
        return (result, obj);
    }
}
public interface ITestRepository
{
    public (string, TestObject) Test(string data, TestObject obj);
}