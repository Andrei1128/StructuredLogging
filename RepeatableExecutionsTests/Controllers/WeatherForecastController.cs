using Microsoft.AspNetCore.Mvc;
using RepeatableExecutionsTests.Entities;
using RepeatableExecutionsTests.Services;
using StructuredLogging.Attributes;

namespace RepeatableExecutionsTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private WeatherForecastService _weatherForecastService;
        public WeatherForecastController()
        {
            _weatherForecastService = new WeatherForecastService();
        }
        [StructuredLogging]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecastService.Get();
        }
    }
}