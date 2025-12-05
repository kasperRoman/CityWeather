using CityWeather.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using CityWeather.Configuration;

namespace CityWeather.Tests
{
    public class WeatherServiceTest
    {
        [Fact]
        public void BuildRequestUrl_ReturnsCorrectUrl()
        {
            var options = Options.Create(new OpenWeatherOptions { ApiKey = "testkey" });
            var service = new WeatherService(new HttpClient(), options, NullLogger<WeatherService>.Instance);

            var method = typeof(WeatherService).GetMethod("BuildRequestUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            string url = (string)method.Invoke(service, ["Lviv"])!;

            Assert.Contains("q=Lviv", url);
            Assert.Contains("appid=testkey", url);
            Assert.Contains("units=metric", url);
        }
    }
}