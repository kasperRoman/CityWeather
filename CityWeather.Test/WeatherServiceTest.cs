using CityWeather.Configuration;
using CityWeather.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;


namespace CityWeather.Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public void BuildRequestUrl_ShouldReturnCorrectUrl()
        {
            var options = Options.Create(new OpenWeatherOptions
            {
                ApiKey = "TEST_KEY"
            });

            var httpClient = new HttpClient();
            var logger = NullLogger<WeatherService>.Instance;

            var service = new WeatherService(httpClient, options, logger);
            
            string url = service.BuildRequestUrl("New York");

            Assert.Contains("q=New%20York", url);
            Assert.Contains("appid=TEST_KEY", url);
            Assert.Contains("&units=metric", url);
            Assert.Contains("&lang=en", url);
        }
    }
}