namespace CityWeather.Configuration
{
    public class OpenWeatherOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;

        public int TimeoutSeconds { get; set; }
    }
}

