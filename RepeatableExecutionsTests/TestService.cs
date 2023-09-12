namespace RepeatableExecutionsTests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _repository;
        private readonly ITestRepository2 _repository2;
        public TestService(ITestRepository repository, ITestRepository2 repository2)
        {
            _repository = repository;
            _repository2 = repository2;
        }
        public string Test(string data)
        {
            var result = _repository.Test(data + "_Repository");
            var result2 = _repository2.Test(data + "_Repository2");
            return result + "_" + result2;
        }
    }
    public interface ITestService
    {
        public string Test(string data);
    }
}