namespace RepeatableExecutionsTests
{
    public class TestAboveRepository : ITestAboveRepository
    {
        public string Test(string data)
        {
            var result = data;
            return result;
        }
    }
    public interface ITestAboveRepository
    {
        public string Test(string data);
    }
}