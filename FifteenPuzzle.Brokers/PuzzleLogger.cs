namespace FifteenPuzzle.Brokers;

using System.Reflection;
using Serilog;
using Serilog.Core;

public class PuzzleLogger
{
    private readonly Logger _logger;

    public PuzzleLogger(LoggingConfiguration loggingConfiguration)
	{
		string now = DateTime.Now.ToString("yyyyMMdd_HHmm");
		_logger = new LoggerConfiguration()
            .WriteTo.File($"{loggingConfiguration.LogDirectoryPath}/{Assembly.GetExecutingAssembly().GetName().Name}.{now}.log")
			.Enrich.FromLogContext()
			//.WriteTo.ColoredConsole()
            .CreateLogger();
	}

    public void LogInformation(string message) => _logger.Information(message);

    public void LogWarning(string message) => _logger.Warning(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogError(string message, Exception exception) => _logger.Error(exception, message);
}
