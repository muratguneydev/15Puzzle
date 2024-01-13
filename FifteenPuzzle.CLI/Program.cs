namespace FifteenPuzzle.CLI;

using FifteenPuzzle.Solvers;
using FifteenPuzzle.CLI.Commands;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Brokers;
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
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }

    private static void ConfigureServices(IServiceCollection services)
	{
		// build config
		var configuration = new ConfigurationBuilder()
		    //.AddEnvironmentVariables()
			//.AddCommandLine(args)
			.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
		    .Build();

		// settings
		services.Configure<QLearningHyperparameters>(configuration.GetSection(nameof(QLearningHyperparameters)));
		services.Configure<QLearningSystemConfiguration>(configuration.GetSection(nameof(QLearningSystemConfiguration)));
		services.Configure<LoggingConfiguration>(configuration.GetSection(nameof(LoggingConfiguration)));

		// add commands:
		services.AddTransient<Command, DepthFirstCommand>();

		// add services:
		services.AddTransient(_ => AnsiConsole.Console);

		services.AddTransient<DepthFirstSolver>();
		services.AddTransient<Renderer>();

		services.AddTransient<Random>();
		services.AddTransient<PuzzleLogger>();

		services.AddTransient<QLearning>();

		services.AddTransient<QValueReader>();
		services.AddTransient<QValueWriter>();
		services.AddTransient<BoardActionQValuesStringConverter>();
		services.AddTransient<FileSystem>();

		services.AddTransient<IRewardStrategy,GreedyManhattanDistanceRewardStrategy>();
		services.AddTransient<IActionSelectionPolicyFactory,EpsilonGreedyActionSelectionPolicyFactory>();
		services.AddTransient<NonRepeatingActionSelectionPolicy>();
		services.AddTransient<BoardFactory>();
		services.AddTransient<BoardActionFactory>();
	}
}
