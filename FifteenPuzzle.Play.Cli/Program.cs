﻿namespace FifteenPuzzle.Play.Cli;

using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

public class Program
{
	static async Task Main(string[] args)
    {
		var configuration = ConfigurationProvider.BuildConfiguration();
		var serviceConfigurator = new ServiceConfigurator(configuration);
        using var serviceProvider = serviceConfigurator.GetServiceProvider();
		//dotnet run --  --help
        var rootCommand = GetRootCommand(serviceProvider);
        await rootCommand.InvokeAsync(args);
    }

    private static RootCommand GetRootCommand(ServiceProvider serviceProvider)
    {
        var rootCommand = new RootCommand("15-puzzle player.");

        var commands = serviceProvider.GetServices<Command>();
        commands.ToList().ForEach(rootCommand.Add);

        return rootCommand;
    }
}
