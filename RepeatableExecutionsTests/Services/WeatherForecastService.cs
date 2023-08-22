using RepeatableExecutionsTests.Consts;
using RepeatableExecutionsTests.Entities;
using StructuredLogging.Attributes;

namespace RepeatableExecutionsTests.Services
{
    public class WeatherForecastService
    {
        [Log]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries.Data[Random.Shared.Next(Summaries.Data.Length)]
            })
            .ToArray();
        }
    }
}
