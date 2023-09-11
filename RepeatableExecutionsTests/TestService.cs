namespace RepeatableExecutionsTests
{
    public class TestRepository : ITestRepository
    {
        public string Test(string data)
        {
            var result = data;
            return result;
        }
    }
    public interface ITestRepository
    {
        public string Test(string data);
    }
}