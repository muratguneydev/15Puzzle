namespace FifteenPuzzle.Play.Cli.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Cli.Tools.BoardRendering;

public class NewGameCommand : Command
{
    private readonly GameApiClient _apiClient;
    private readonly SolversApiClient _solversApiClient;
    private readonly ConsoleBoardRenderer _renderer;
    private readonly SuggestionsRenderer _suggestionsRenderer;
    private readonly PuzzleLogger _logger;
    
    //dotnet run play

    public NewGameCommand(GameApiClient apiClient, SolversApiClient solversApiClient,
			ConsoleBoardRenderer renderer, SuggestionsRenderer suggestionsRenderer,
			PuzzleLogger logger)
        : base("new", "Creates a new game board.")
    {
        _apiClient = apiClient;
        _solversApiClient = solversApiClient;
        _renderer = renderer;
        _suggestionsRenderer = suggestionsRenderer;
        _logger = logger;

		this.SetHandler(Execute);
    }

    private async Task Execute()
    {
		var board = await  _apiClient.GetNewBoard();
		var suggestions = await _solversApiClient.GetActionQualityValues(board);
		_suggestionsRenderer.Render(suggestions);
		_renderer.Render(board);
    }
}
