namespace FifteenPuzzle.Play.Cli.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Cli.Tools.BoardRendering;

public class PlayCommand : Command
{
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    
    //dotnet run play

    public PlayCommand(ConsoleBoardRenderer renderer, PuzzleLogger logger)
        : base("play", "Play 15 Puzzle with the help of previously learned data.")
    {
        _renderer = renderer;
        _logger = logger;

		var newGameOption = new Option<bool>(new[] { "--newGame", "-n" }, () => false, "Start a new game");
        AddOption(newGameOption);
        this.SetHandler(Execute, newGameOption);

        //this.SetHandler(Execute);
    }

    private Task Execute(bool newGame)
    {
		if (newGame)
		{
			
		}
    	return Task.CompletedTask;
    }
}
