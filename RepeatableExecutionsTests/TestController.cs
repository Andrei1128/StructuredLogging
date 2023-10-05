using Logging.Interceptors;
using Microsoft.AspNetCore.Mvc;
using ILogger = Logging.Interceptors.ILogger;

namespace RepeatableExecutionsTests
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private ITestService _testService;
        private ILogger _logger;
        //private Writer _writer;
        public TestController(ITestService testService, ILogger logger
            //, Writer writer
            )
        {
            //_writer = writer;
            _testService = testService;
            _logger = logger;
        }
        [HttpPost]
        [ServiceFilter(typeof(StructuredLoggingAttribute))]
        public string GetWeatherEndpoint()
        {
            _logger.Information("Test_Controller");
            var result = _testService.Test("Ok_Service");
            _logger.Information("Test_Controller");
            return "Test_Controller";
        }
    }
}