using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CityWeather
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
        public List<WeatherInfo> Weather { get; set; } = new();
    }

    public class Sys
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }

    public class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
