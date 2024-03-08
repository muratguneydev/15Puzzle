namespace FifteenPuzzle.Play.Cli;

using FifteenPuzzle.Brokers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.CommandLine;
using FifteenPuzzle.Cli.Tools.BoardRendering;
using FifteenPuzzle.Play.Cli.Commands;
using Microsoft.Extensions.Options;

public class ServiceConfigurator
{
    private readonly IConfigurationRoot _configuration;

    public ServiceConfigurator(IConfigurationRoot configuration) => _configuration = configuration;

    public ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        RegisterSettings(services, _configuration);
        RegisterCommands(services);
        services.AddTransient(_ => AnsiConsole.Console);
        RegisterServices(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<ConsoleBoardRenderer>();
        services.AddSingleton<PuzzleLogger>();

        services.AddTransient<GameApiClient>();
		services.AddHttpClient(GameApiClient.Name, (serviceProvider, httpClient) =>
		{
			var options = serviceProvider.GetRequiredService<ApiSettings>();
			httpClient.BaseAddress = new Uri(options.BaseUrl);
		});
    }

    private static void RegisterCommands(IServiceCollection services)
    {
        services.AddTransient<Command, NewGameCommand>();
    }

    private static void RegisterSettings(IServiceCollection services, IConfigurationRoot configuration)
    {
        ConfigurePOCO<LoggingConfiguration>(services, configuration.GetSection(nameof(LoggingConfiguration)));
        ConfigurePOCO<ApiSettings>(services, configuration.GetSection(nameof(ApiSettings)));
    }

	/// <summary>Registers the setting classes as POCO rather than IOption<T>.</summary>
	private static TConfig ConfigurePOCO<TConfig>(IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
	{
		if (services == null) throw new ArgumentNullException(nameof(services));
		if (configuration == null) throw new ArgumentNullException(nameof(configuration));

		var config = new TConfig();
		configuration.Bind(config);
		services.AddSingleton(config);
		return config;
	}
}
