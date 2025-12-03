using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWeather
{

        public class OpenWeatherOptions
        {
            public string ApiKey { get; set; } = string.Empty;
            public string BaseUrl { get; set; } = string.Empty;

            public  int TimeoutSeconds { get; set; } 
    }

    }

