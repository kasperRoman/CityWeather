using CityWeather.Configuration;
using CityWeather.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;

namespace CityWeather.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_response.StatusCode)
            {
                Content = _response.Content
            });
        }
    }
    public class WeatherService_StatusCodeTests
    {
        [Fact]

        public async Task GetWeatherAsync_ReturnNull_AndLogsError__OnUnauthorized_401()
        {
            var options = Options.Create(new OpenWeatherOptions { ApiKey = "testkey" });

            var response401 = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("")
            };
            var handler = new FakeHttpMessageHandler(response401);
            var httpClient = new HttpClient(handler);
            {
                httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            }

            var loggerMock = new Mock<ILogger<WeatherService>>();

            var service = new WeatherService(httpClient, options, loggerMock.Object);

            var result = await service.GetWeatherAsync("Lviv");

            Assert.Null(result);

            loggerMock.Verify(
                x => x.Log(
                     LogLevel.Error,
                     It.IsAny<EventId>(),
                     It.Is<It.IsAnyType>((state, type) => state.ToString()!.Contains("Invalid API key (401).")),
                     It.IsAny<Exception>(),
                     It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                 ),
                 Times.Once
             );
        }
        [Fact]
        public async Task GetWeatherAsync_ReturnsNull_AndLogsError_OnForbidden_403()
        {
            var options = Options.Create(new OpenWeatherOptions { ApiKey = "testkey" });

            var response403 = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent("")
            };

            var handler = new FakeHttpMessageHandler(response403);
            var httpClient = new HttpClient(handler);
            {      
                httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            }

            var loggerMock = new Mock<ILogger<WeatherService>>();

            var service = new WeatherService(httpClient, options, loggerMock.Object);

            
            var result = await service.GetWeatherAsync("Lviv");

            Assert.Null(result);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, type) => state.ToString()!.Contains("API key is blocked (403)")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                    ),
                Times.Once);
        }
        [Fact]
        public async Task GetWeatherAsync_ReturnsNull_AndLogsWarning_OnNotFound_404()
        {
            var options = Options.Create(new OpenWeatherOptions { ApiKey = "testkey" });

            var response404 = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("")
            };

            var handler = new FakeHttpMessageHandler(response404);
            var httpClient = new HttpClient(handler);
            {
                httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            }

            var loggerMock = new Mock<ILogger<WeatherService>>();

            var service = new WeatherService(httpClient, options, loggerMock.Object);

            var result = await service.GetWeatherAsync("InvalidCity");

            Assert.Null(result);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, type) => state.ToString()!.Contains("not found") ),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }

}
