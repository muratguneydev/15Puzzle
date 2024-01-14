namespace FifteenPuzzle.CLI;
using Microsoft.Extensions.Configuration;

public class ConfigurationProvider
{
	public static IConfigurationRoot BuildConfiguration() =>
		new ConfigurationBuilder()
			//.AddEnvironmentVariables()
			//.AddCommandLine(args)
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false)
			.Build();
}
