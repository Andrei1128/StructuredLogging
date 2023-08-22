using RepeatableExecutionsTests.Attributes;
using RepeatableExecutionsTests.Consts;
using RepeatableExecutionsTests.Entities;

namespace RepeatableExecutionsTests.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        [Log]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine("In Get Method");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries.Data[Random.Shared.Next(Summaries.Data.Length)]
            })
            .ToArray();
        }
    }
    public interface IWeatherForecastService
    {
        public IEnumerable<WeatherForecast> Get();
    }
}
