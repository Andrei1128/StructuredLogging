using Logging.Attributes;
using Microsoft.AspNetCore.Mvc;
using RepeatableExecutionsTests.Services;

namespace RepeatableExecutionsTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IWeatherForecastService _weatherForecastService;
        private IWeatherForecastService2 _weatherForecastService2;
        public WeatherForecastController(IWeatherForecastService weatherForecastService, IWeatherForecastService2 weatherForecastService2)
        {
            _weatherForecastService = weatherForecastService;
            _weatherForecastService2 = weatherForecastService2;
        }
        [ServiceFilter(typeof(StructuredLoggingAttribute))]
        [HttpPost]
        public string GetWeatherEndpoint([FromBody] int payload)
        {
            return _weatherForecastService.GetWeather("asdasd", 12);
        }
        [ServiceFilter(typeof(StructuredLoggingAttribute))]
        [HttpPut]
        public string Get2([FromBody] int payload)
        {
            return _weatherForecastService.GetWeather2("asdasd", 12);
        }
    }
}