namespace FifteenPuzzle.Brokers;

public class LoggingConfiguration
{
    public LoggingConfiguration(string logFilePath) => LogFilePath = logFilePath;

    public string LogFilePath { get; }
}