using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWeather
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherAsync(string city);
    }

    public class WeatherService : IWeatherService  //реалізує інтерфейс
    {
        private readonly HttpClient _client;
        private readonly ILogger<WeatherService> _logger;
        private readonly OpenWeatherOptions _options;

        public WeatherService(HttpClient client, IOptions<OpenWeatherOptions> options, ILogger<WeatherService> logger)
        {
            _client = client;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string city)
        {
            string url = BuildRequestUrl(city);

            _logger.LogInformation($"Executing request: {url}");

            using HttpResponseMessage response = await _client.GetAsync(url);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    _logger.LogError("Invalid API key (401)");
                    return null;

                case HttpStatusCode.Forbidden:
                    _logger.LogError("API key is blocked (403)");
                    return null;

                case HttpStatusCode.NotFound:
                    _logger.LogWarning("City not found. Check spelling.");
                    return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Unexpected error: {response.StatusCode}");
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<WeatherResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error.");
                return null;
            }
        }

        private string BuildRequestUrl(string city)
        {
            return $"weather?q={Uri.EscapeDataString(city)}&appid={_options.ApiKey}&units=metric&lang=en";
        }
    }
}
