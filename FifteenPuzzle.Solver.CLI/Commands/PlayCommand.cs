namespace FifteenPuzzle.Solver.Cli.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Cli.Tools.BoardRendering;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class PlayCommand : Command
{
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    
    //dotnet run play


	//TODO: FifteenPuzzle.Solver.Cli
	//FifteenPuzzle.Play.CLI -> each command is a dedicated play command.
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
