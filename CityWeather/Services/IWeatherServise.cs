using CityWeather.Dtos;

namespace CityWeather.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherAsync(string city);
    }
}
