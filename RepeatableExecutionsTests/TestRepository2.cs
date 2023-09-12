namespace RepeatableExecutionsTests
{
    public class TestRepository2 : ITestRepository2
    {
        public string Test(string data)
        {
            var result = data;
            return result;
        }
    }
    public interface ITestRepository2
    {
        public string Test(string data);
    }
}