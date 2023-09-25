using Logging.Interceptors;
using Microsoft.AspNetCore.Mvc;

namespace RepeatableExecutionsTests
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private ITestService _testService;
        private ILogger2 _logger;
        public TestController(ITestService testService, ILogger2 logger)
        {
            _testService = testService;
            _logger = logger;
        }
        [HttpPost]
        [ServiceFilter(typeof(StructuredLoggingAttribute))]
        public string GetWeatherEndpoint()
        {
            var result = _testService.Test("Ok_Service");
            _logger.Information("conrollertest");
            return result;
        }
    }
}