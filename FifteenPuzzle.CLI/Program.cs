namespace FifteenPuzzle.CLI;

using Spectre.Console;
using FifteenPuzzle.Solvers;
using Microsoft.Extensions.DependencyInjection;

using System.CommandLine;
using FifteenPuzzle.CLI.Commands;

public class Program
{
	static async Task Main(string[] args)
	{
		// create service collection
		var services = new ServiceCollection();
		ConfigureServices(services);

		// create service provider
		using var serviceProvider = services.BuildServiceProvider();

		// entry to run app
		var commands = serviceProvider.GetServices<Command>();
		var rootCommand = new RootCommand("15-puzzle reinforcement learning and solving utility.");
		commands.ToList().ForEach(rootCommand.AddCommand);

		await rootCommand.InvokeAsync(args);
	}

	private static void ConfigureServices(IServiceCollection services)
	{
		// build config
		// var configuration = new ConfigurationBuilder()
		//     .AddEnvironmentVariables()
		//     .Build();

		// settings
		//services.Configure<FakeWeatherServiceSettings>(configuration.GetSection("Weather"));

		// add commands:
		//services.AddTransient<Command, CurrentCommand>();
		services.AddTransient<Command, DepthFirstCommand>();

		// add services:
		services.AddTransient<DepthFirstSolver>();
		services.AddTransient<Renderer>();
		services.AddTransient(_ => AnsiConsole.Console);
	}
}
