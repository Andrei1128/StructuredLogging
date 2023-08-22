using RepeatableExecutionsTests.Attributes;

namespace RepeatableExecutionsTests.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        [Log]
        public string Get(string data, int ora)
        {
            return "asdasdasd";
        }
    }
    public interface IWeatherForecastService
    {
        public string Get(string data, int ora);
    }
}
