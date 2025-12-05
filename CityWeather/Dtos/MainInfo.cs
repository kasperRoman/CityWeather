using System.Text.Json.Serialization;

namespace CityWeather.Dtos
{
    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }
}
