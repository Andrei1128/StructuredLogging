using Microsoft.AspNetCore.Mvc;

namespace RepeatableExecutionsTests
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        [HttpPost]
        //[ServiceFilter(typeof(StructuredLoggingAttribute))]
        public string GetWeatherEndpoint([FromBody] string payload)
        {
            return _testService.Test("Ok_Service");
        }
    }
}