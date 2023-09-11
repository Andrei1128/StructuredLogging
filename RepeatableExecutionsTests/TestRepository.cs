namespace RepeatableExecutionsTests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _repository;
        public TestService(ITestRepository repository)
        {
            _repository = repository;
        }
        public string Test(string data)
        {
            var result = _repository.Test(data + "_Repository");
            throw new Exception("Ok_Exception");
            return result;
        }
    }
    public interface ITestService
    {
        public string Test(string data);
    }
}