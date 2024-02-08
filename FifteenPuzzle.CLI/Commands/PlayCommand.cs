namespace FifteenPuzzle.CLI.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class PlayCommand : Command
{
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    
    //dotnet run play

    public PlayCommand(QLearning qLearning, ConsoleBoardRenderer renderer, PuzzleLogger logger)
        : base("play", "Play 15 Puzzle with the help of previously learned data.")
    {
        _renderer = renderer;
        _logger = logger;

        this.SetHandler(Execute);
    }

    private Task Execute()
    {
    	return Task.CompletedTask;
    }
}
