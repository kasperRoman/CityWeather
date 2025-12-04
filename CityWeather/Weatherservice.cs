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

    public class WeatherService : IWeatherService  
    {
        private readonly HttpClient _client;
        private readonly ILogger<WeatherService> _logger;
        private readonly OpenWeatherOptions _options;

        private static readonly JsonSerializerOptions _jsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        public WeatherService(HttpClient client, IOptions<OpenWeatherOptions> options, ILogger<WeatherService> logger)
        {
            _client = client;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string city)
        {
            string url = BuildRequestUrl(city);

            _logger.LogInformation("Requesting weather for {City}. URL: {Url}", city, url);

            try
            {
                using HttpResponseMessage response = await _client.GetAsync(url);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        _logger.LogError("Invalid API key (401).");
                        return null;

                    case HttpStatusCode.Forbidden:
                        _logger.LogError("API key is blocked (403).");
                        return null;

                    case HttpStatusCode.NotFound:
                        _logger.LogWarning("City '{City}' not found.", city);
                        return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Unexpected HTTP error {StatusCode}", response.StatusCode);
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonSerializer.Deserialize<WeatherResponse>(json, _jsonOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "JSON deserialization error.");
                    return null;
                }
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError("Request timeout.");
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during weather request.");
                return null;
            }
        }

        private string BuildRequestUrl(string city)
        {
            return $"weather?q={Uri.EscapeDataString(city)}&appid={_options.ApiKey}&units=metric&lang=en";
        }
    }
}
