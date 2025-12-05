using CityWeather.Services;
using Microsoft.Extensions.Logging;

namespace CityWeather.Core
{
    public class WeatherApp
    {
        private readonly IWeatherService _service;
        private readonly ILogger<WeatherApp> _logger;

        public WeatherApp(IWeatherService service, ILogger<WeatherApp> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Write("Enter city name (or 'exit'): ");
                string? rawInput = Console.ReadLine();

                string cityInput = rawInput?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(cityInput))
                {
                    _logger.LogWarning("City was not entered.");
                    continue;
                }

                if (cityInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Exiting program.");
                    break;
                }

                var weather = await _service.GetWeatherAsync(cityInput);

                if (weather == null)
                {
                    Console.WriteLine("Weather data not found. Try again.");
                    continue;
                }

               
                string description = weather.Weather.FirstOrDefault()?.Description ?? "Unknown";

                _logger.LogInformation($"City: {weather.Name}, Country: {weather.Country}");
                _logger.LogInformation($"Temperature: {weather.Main.Temp}°C");
                _logger.LogInformation($"Humidity: {weather.Main.Humidity}%");
                _logger.LogInformation($"Wind: {weather.Wind.Speed} m/s");
                _logger.LogInformation($"Conditions: {description}");

                Console.WriteLine();
            }
        }
    }
}