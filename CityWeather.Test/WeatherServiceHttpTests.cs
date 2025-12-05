using CityWeather.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;

namespace CityWeather.Tests
{
    public class WeatherServiceHttpTests
    {
        private class FakeHandler(HttpStatusCode statusCode) : HttpMessageHandler
        { 
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(statusCode));
            }
        }
        [Fact]
        public async Task GetWeatherAsync_NotFound_ReturnsNull()
        {
            var client = new HttpClient(new FakeHandler(HttpStatusCode.NotFound))
            {
                BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/")
            };
            var options = Options.Create(new OpenWeatherOptions { ApiKey = "test" });
            var service = new WeatherService(client, options, NullLogger<WeatherService>.Instance);

            var result = await service.GetWeatherAsync("InvalidCity");

            Assert.Null(result);
            
        }

        [Fact]
        public async Task GetWeatherAsync_Timeout_ReturnsNull()
        {
            var handler = new FakeHandler(HttpStatusCode.OK);
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/"),
                Timeout = TimeSpan.FromMilliseconds(1)
            };

            var options = Options.Create(new OpenWeatherOptions { ApiKey = "test" });
            var service = new WeatherService(client, options, NullLogger<WeatherService>.Instance);

            var result = await service.GetWeatherAsync("Kyiv");

            Assert.Null(result);
        }
    }
}