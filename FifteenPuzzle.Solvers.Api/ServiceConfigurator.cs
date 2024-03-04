namespace FifteenPuzzle.Solvers.Api;

using FifteenPuzzle.Brokers;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
    {
        RegisterSettings(services, configuration);
        RegisterServices(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<PuzzleLogger>();
        AddQValueStorageDependencies(services);
    }

    private static void AddQValueStorageDependencies(IServiceCollection services)
    {
        services.AddTransient<QValueReader>();
        services.AddTransient<QualityValueRepository>();
        services.AddTransient<BoardActionQValuesStringConverter>();
        services.AddTransient<FileSystem>();
    }

    private static void RegisterSettings(IServiceCollection services, IConfigurationRoot configuration)
    {
        ConfigurePOCO<QLearningSystemConfiguration>(services, configuration.GetSection(nameof(QLearningSystemConfiguration)));
        ConfigurePOCO<LoggingConfiguration>(services, configuration.GetSection(nameof(LoggingConfiguration)));
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
