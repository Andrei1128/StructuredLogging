using Logging.Attributes;

namespace RepeatableExecutionsTests.Services
{
    public class WeatherForecastService2 : IWeatherForecastService2
    {
        [Log]
        public string Get(string data, int ora)
        {
            return "asdasdasd";
        }
    }
    public interface IWeatherForecastService2
    {
        public string Get(string data, int ora);
    }
}
