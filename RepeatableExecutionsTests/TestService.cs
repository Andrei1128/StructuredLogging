using RepeatableExecutionsTests;
using ILogger = Logging.Logging.Interceptors.ILogger;
namespace Service;
public class TestService : ITestService
{
    private readonly ITestRepository _repository;
    private readonly ITestRepository2 _repository2;
    private readonly ILogger _logger;
    //public TestService(
    //    ITestRepository repository,
    //    ITestRepository2 repository2, ILogger logger)
    //{
    //    _repository = repository;
    //    _repository2 = repository2;
    //    _logger = logger;
    //}
    //public string Test(string data, int number, TestObject obj)
    //{
    //    //var result = _repository.Test(data + "_Repository", obj);
    //    //_repository2.Test(data + "_Repository2", out string result2);
    //    return "Test_Service";
    //}

    public override string Test(string data, int number, TestObject obj)
    {
        return "Test_Service";
    }
    public override string testSucces()
    {
        return "neoriginal!";
    }
}
public abstract class ITestService
{
    public abstract string Test(string data, int number, TestObject obj);
    public virtual string testSucces()
    {
        return "successOriginal";
    }
    public string testFailed()
    {
        return "faild..";
    }
}