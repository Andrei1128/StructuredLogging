using Microsoft.AspNetCore.Mvc;
using RepeatableExecutionsTests.Attributes;
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
        [StructuredLogging]
        [HttpPost]
        public string Get([FromBody] int payload)
        {
            return _weatherForecastService.Get("asdasd", 12);
        }
        [StructuredLogging]
        [HttpPut]
        public string Get2([FromBody] int payload)
        {
            return _weatherForecastService2.Get("asdasd", 12);
        }
    }
}