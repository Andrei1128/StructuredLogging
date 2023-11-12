using Logging.Logging.Interceptors;
using Logging.Replaying.Objects;
using Microsoft.AspNetCore.Mvc;
using RepeatableExecutionsTests;
using Service;
using ILogger = Logging.Logging.Interceptors.ILogger;

namespace Tests.Objects;
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private ITestService _testService;
    private ILogger _logger;
    public TestController(ITestService testService, ILogger logger)
    {
        _testService = testService;
        _logger = logger;
    }
    [HttpPost]
    [ServiceFilter(typeof(StructuredLoggingAttribute))]
    public string GetWeatherEndpoint(string data, int number, [FromBody] TestObject obj)
    {
        var result = _testService.Test(data, 1, obj);
        var result1 = _testService.testSucces();
        var result2 = _testService.testFailed();
        return data + " " + number + " " + obj;
    }
    [HttpGet]
    public void Replay()
    {
        var replayer = new Replayer();
        replayer.Replay();
    }
}