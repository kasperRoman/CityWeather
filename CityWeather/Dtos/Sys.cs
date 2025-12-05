using System.Text.Json.Serialization;

namespace CityWeather.Dtos
{
    public class Sys
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }
}
