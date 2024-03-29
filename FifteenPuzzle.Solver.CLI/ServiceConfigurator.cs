namespace FifteenPuzzle.Solver.Cli;

using FifteenPuzzle.Brokers;
using FifteenPuzzle.Solver.Cli.Commands;
using FifteenPuzzle.Solvers;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.CommandLine;
using FifteenPuzzle.Game;
using FifteenPuzzle.Cli.Tools.BoardRendering;

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
        RegisterServices(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient(_ => AnsiConsole.Console);
		services.AddTransient<TableBoardRenderer>();
        services.AddTransient<FlatBoardRenderer>();
        services.AddTransient<ConsoleBoardRenderer>();
		
        services.AddTransient<Random>();
        services.AddSingleton<PuzzleLogger>();

        AddQValueStorageDependencies(services);
        AddQLearningDependencies(services);

        services.AddTransient<DepthFirstSolver>();
    }

    private static void AddQLearningDependencies(IServiceCollection services)
    {
        services.AddTransient<QLearning>();
        services.AddTransient<QValueCalculator>();
        services.AddSingleton<BoardTracker>();
        services.AddTransient<IRewardStrategy, GreedyManhattanDistanceRewardStrategy>();
        services.AddTransient<IActionSelectionPolicyFactory, EpsilonGreedyActionSelectionPolicyFactory>();
        services.AddTransient<NonRepeatingActionSelectionPolicy>();
        services.AddTransient<BoardFactory>();
        services.AddTransient<BoardActionFactory>();
    }

    private static void AddQValueStorageDependencies(IServiceCollection services)
    {
        services.AddTransient<QValueReader>();
        services.AddTransient<QValueWriter>();
        services.AddTransient<BoardActionQValuesStringConverter>();
        services.AddTransient<FileSystem>();
    }

    private static void RegisterCommands(IServiceCollection services)
    {
        services.AddTransient<Command, DepthFirstCommand>();
        services.AddTransient<Command, QLearningCommand>();
    }

    private static void RegisterSettings(IServiceCollection services, IConfigurationRoot configuration)
    {
		ConfigurePOCO<QLearningHyperparameters>(services, configuration.GetSection(nameof(QLearningHyperparameters)));
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