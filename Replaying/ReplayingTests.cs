using Logging.Objects;
using Moq;
using Newtonsoft.Json;
using RepeatableExecutionsTests;
using ILogger = Logging.Interceptors.ILogger;

namespace Replaying;
public class ReplayingTests
{
    //  "Entry": {
    //  "Time": "2023-10-16T20:25:32.3975514+03:00",
    //  "Class": "RepeatableExecutionsTests.TestController",
    //  "Method": "GetWeatherEndpoint",
    //  "Input": {}
    //  },
    //  "Exit": {
    //  "Time": "2023-10-16T20:25:32.4013371+03:00",
    //  "Output": "Test_Controller"
    //  },
    [Fact]
    public void Replay()
    {
        string serializedLog = File.ReadAllText("..\\..\\..\\..\\Logging\\logs\\log-202310162025293502471");
        Log log = JsonConvert.DeserializeObject<Log>(serializedLog);

        var testServiceMock = new Mock<ITestService>();
        var loggerMock = new Mock<ILogger>();
        var controller = new TestController(testServiceMock.Object, loggerMock.Object);


        string methodToCall = log.Entry.Method;
        var methodInfo = typeof(TestController).GetMethod(methodToCall);
        if (methodInfo != null)
        {
            var result = methodInfo.Invoke(controller, null);
        }
        else throw new InvalidOperationException($"Method '{methodToCall}' does not exist in the controller.");
    }
}