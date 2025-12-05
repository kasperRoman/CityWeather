using System.Text.Json.Serialization;

namespace CityWeather.Dtos
{
    public class WeatherResponse
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; } = new();

        public string Country => Sys?.Country ?? "";

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; } = new();

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; } = [];
    }
}
