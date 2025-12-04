using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWeather.Tests
{
    public class WeatherResponseTests
    {
        [Fact]
        public void Deserialize_ValidJson_ReturnsWeatherResponse()
        {
            string json = @"{
            ""name"": ""Kyiv"",
            ""sys"": { ""country"": ""UA"" },
            ""main"": { ""temp"": 5.5, ""humidity"": 80 },
            ""wind"": { ""speed"": 3.2 },
            ""weather"": [ { ""description"": ""clear sky"" } ]
        }";

            var response = JsonSerializer.Deserialize<WeatherResponse>(json);

            Assert.NotNull(response);
            Assert.Equal("Kyiv", response.Name);
            Assert.Equal("UA", response.Sys.Country);
            Assert.Equal(5.5, response.Main.Temp);
            Assert.Equal(80, response.Main.Humidity);
            Assert.Equal(3.2, response.Wind.Speed);
            Assert.Equal("clear sky", response.Weather[0].Description);

        }
    }
}