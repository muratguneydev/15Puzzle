namespace FifteenPuzzle.CLI;

using FifteenPuzzle.Brokers;
using FifteenPuzzle.CLI.Commands;
using FifteenPuzzle.Solvers;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.CommandLine;

public class Program
{
	static async Task Main(string[] args)
    {
        using var serviceProvider = GetServiceProvider();
        var rootCommand = GetRootCommand(serviceProvider);
        await rootCommand.InvokeAsync(args);
    }

    private static RootCommand GetRootCommand(ServiceProvider serviceProvider)
    {
        var rootCommand = new RootCommand("15-puzzle reinforcement learning and solving utility.");

        var commands = serviceProvider.GetServices<Command>();
        commands.ToList().ForEach(rootCommand.Add);

        return rootCommand;
    }

    private static ServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = BuildConfiguration();
		//services.AddSingleton<IConfiguration>(configuration);

        RegisterSettings(services, configuration);
        RegisterCommands(services);
        services.AddTransient(_ => AnsiConsole.Console);
		//services.AddSingleton<IConfiguration>(configuration);
        RegisterServices(services);
    }

    private static IConfigurationRoot BuildConfiguration() =>
		new ConfigurationBuilder()
			//.AddEnvironmentVariables()
			//.AddCommandLine(args)
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<DepthFirstSolver>();
        services.AddTransient<Renderer>();

        services.AddTransient<Random>();
        services.AddTransient<PuzzleLogger>();

        services.AddTransient<QLearning>();

        services.AddTransient<QValueReader>();
        services.AddTransient<QValueWriter>();
        services.AddTransient<BoardActionQValuesStringConverter>();
        services.AddTransient<FileSystem>();

        services.AddTransient<IRewardStrategy, GreedyManhattanDistanceRewardStrategy>();
        services.AddTransient<IActionSelectionPolicyFactory, EpsilonGreedyActionSelectionPolicyFactory>();
        services.AddTransient<NonRepeatingActionSelectionPolicy>();
        services.AddTransient<BoardFactory>();
        services.AddTransient<BoardActionFactory>();
    }

    private static void RegisterCommands(IServiceCollection services)
    {
        services.AddTransient<Command, DepthFirstCommand>();
        services.AddTransient<Command, QLearningCommand>();
    }

    private static void RegisterSettings(IServiceCollection services, IConfigurationRoot configuration)
    {
        // services.Configure<QLearningHyperparameters>(configuration.GetSection(nameof(QLearningHyperparameters)));
        // services.Configure<QLearningSystemConfiguration>(configuration.GetSection(nameof(QLearningSystemConfiguration)));
        // services.Configure<LoggingConfiguration>(configuration.GetSection(nameof(LoggingConfiguration)));
		services.ConfigurePOCO<QLearningHyperparameters>(configuration.GetSection(nameof(QLearningHyperparameters)));
        services.ConfigurePOCO<QLearningSystemConfiguration>(configuration.GetSection(nameof(QLearningSystemConfiguration)));
        services.ConfigurePOCO<LoggingConfiguration>(configuration.GetSection(nameof(LoggingConfiguration)));
    }
}

public static class ServiceCollectionExtensions
{
	/// <summary>Registers the setting classes as POCO rather than IOption<T>.</summary>
    public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var config = new TConfig();
        configuration.Bind(config);
        services.AddSingleton(config);
        return config;
    }
}