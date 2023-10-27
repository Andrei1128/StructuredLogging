using Logging.Objects;
using Moq;
using Newtonsoft.Json;
using RepeatableExecutionsTests;
using ILogger = Logging.Interceptors.ILogger;

namespace Replaying;
public class ReplayingTests
{
    [Fact]
    public void Replay()
    {
        string serializedLog = File.ReadAllText("..\\..\\..\\..\\Logging\\logs\\log-202310271820317921424");
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        Log? log = JsonConvert.DeserializeObject<Log>(serializedLog, settings);

        var testServiceMock = new Mock<ITestService>();
        var loggerMock = new Mock<ILogger>();
        var controller = new TestController(testServiceMock.Object, loggerMock.Object);


        string methodToCall = log.Entry.Method;
        var methodInfo = typeof(TestController).GetMethod(methodToCall);
        if (methodInfo != null)
        {
            var input = log.Entry.Input;
            var inputobj = input[0] as object[];
            var result = methodInfo.Invoke(controller, inputobj);
        }
        else throw new InvalidOperationException($"Method '{methodToCall}' does not exist in the controller.");
    }
}