namespace RepeatableExecutionsTests
{
    public class TestRepository : ITestRepository
    {
        private readonly ITestAboveRepository _aboveRepository;
        public TestRepository(ITestAboveRepository aboveRepository)
        {
            _aboveRepository = aboveRepository;
        }
        public string Test(string data)
        {
            var result = _aboveRepository.Test(data + "_AboveRepository");
            return result;
        }
    }
    public interface ITestRepository
    {
        public string Test(string data);
    }
}