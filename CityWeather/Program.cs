using CityWeather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;


internal class Program
{ 
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args) //Host статични клас з Microsoft.Extensions.Hosting    CreateDefaultBuilder створює стандартний IHostBuilder
                .ConfigureAppConfiguration(config => // Використовується для того, щоб налаштувати систему конфігурації вашої програми
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) => // це обєкти типу HostBuilderContext ,IServiceCollection  містить context.Configuration яку налаштував вище
                {
                    services.AddOptions<OpenWeatherOptions>() // створює контейнер для налаштувань
                      .Bind(context.Configuration.GetSection("OpenWeatherMap")) //прив’язує значення з конфігурації до властивостей класу
                      .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "API key is required")
                      .ValidateOnStart();

                    services.AddHttpClient<IWeatherService, WeatherService>(client =>
                    {
                        string? baseUrl = context.Configuration["OpenWeatherMap:BaseUrl"];
                        if (baseUrl != null)
                            client.BaseAddress = new Uri(baseUrl);

                        if (int.TryParse(context.Configuration["OpenWeatherMap:TimeoutSeconds"], out int t))
                            client.Timeout = TimeSpan.FromSeconds(t);
                    });
                    services.AddTransient<WeatherApp>();//кожного разу, коли DI-контейнеру потрібен WeatherApp, він створює новий екземпляр цього класу.
                })
                            .ConfigureLogging(logging =>
                            {
                                logging.ClearProviders();//видаляє всі стандартні провайдери.
                                logging.AddConsole();
                            })
            .Build();

        await host.Services.GetRequiredService<WeatherApp>().RunAsync();




    }




}