using Microsoft.AspNetCore.Mvc;
using RepeatableExecutionsTests.Attributes;
using RepeatableExecutionsTests.Entities;
using RepeatableExecutionsTests.Services;

namespace RepeatableExecutionsTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IWeatherForecastService _weatherForecastService;
        public WeatherForecastController(IWeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
        }
        [StructuredLogging]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecastService.Get();
        }
    }
}