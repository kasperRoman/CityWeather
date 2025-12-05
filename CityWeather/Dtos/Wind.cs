using System.Text.Json.Serialization;

namespace CityWeather.Dtos
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }
}
