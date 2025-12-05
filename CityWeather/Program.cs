using CityWeather.Configuration;
using CityWeather.Core;
using CityWeather.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<OpenWeatherOptions>()
                    .Bind(context.Configuration.GetSection("OpenWeatherMap"))
                    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "API key is required")
                    .ValidateOnStart();

                services.AddHttpClient<IWeatherService, WeatherService>(ConfigureHttpClient);

                services.AddTransient<WeatherApp>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
            })
            .Build();

        await host.Services.GetRequiredService<WeatherApp>().RunAsync();
    }

    private static void ConfigureHttpClient(IServiceProvider sp, HttpClient client)
    {
        var config = sp.GetRequiredService<IConfiguration>();

        if (config["OpenWeatherMap:BaseUrl"] is string baseUrl && !string.IsNullOrWhiteSpace(baseUrl))
            client.BaseAddress = new Uri(baseUrl);

        if (int.TryParse(config["OpenWeatherMap:TimeoutSeconds"], out int timeout))
            client.Timeout = TimeSpan.FromSeconds(timeout);
    }
}