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
        //private IServiceProvider _serviceProvider;
        public TestController(ITestService testService, ILogger logger
            //, IServiceProvider serviceProvider
            )
        {
            _testService = testService;
            _logger = logger;
            //_serviceProvider = serviceProvider;
            //var log = _serviceProvider.GetRequiredService<ILog>();
            //Activator.CreateInstance(typeof(Writer), new object[] { log });
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