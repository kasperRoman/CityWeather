using System.Text.Json.Serialization;

namespace CityWeather.Dtos
{
    public class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
