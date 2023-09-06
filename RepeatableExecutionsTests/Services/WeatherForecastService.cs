using Logging.Attributes;

namespace RepeatableExecutionsTests.Services
{
    public class WeatherForecastService : IWeatherForecastService, ITest
    {
        [Log]
        public string GetWeather(string data, int ora)
        {
            return "asdasdasd";
        }
        public string GetWeather2(string data, int ora)
        {
            return "asdasdasd";
        }
    }
    public interface IWeatherForecastService
    {
        public string GetWeather(string data, int ora);
        public string GetWeather2(string data, int ora);
    }
    public interface ITest { }
}
