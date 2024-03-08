namespace FifteenPuzzle.Play.Cli.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Cli.Tools.BoardRendering;

public class NewGameCommand : Command
{
    private readonly GameApiClient _apiClient;
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    
    //dotnet run play

    public NewGameCommand(GameApiClient apiClient, ConsoleBoardRenderer renderer, PuzzleLogger logger)
        : base("new", "Creates a new game board.")
    {
        _apiClient = apiClient;
        _renderer = renderer;
        _logger = logger;

		//var newGameOption = new Option<bool>(new[] { "--newGame", "-n" }, () => false, "Start a new game");
        //AddOption(newGameOption);
        //this.SetHandler(Execute, newGameOption);
        this.SetHandler(Execute);
    }

    private async Task Execute()
    {
		var board = await  _apiClient.GetNewBoard();
		_renderer.Render(board);
    }
}
