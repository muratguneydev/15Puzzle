namespace FifteenPuzzle.Play.Cli.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Cli.Tools.BoardRendering;

public class MoveCommand : Command
{
    private readonly GameApiClient _apiClient;
    private readonly SolversApiClient _solversApiClient;
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    
    //dotnet run move -n 6

    public MoveCommand(GameApiClient apiClient, SolversApiClient solversApiClient,
			ConsoleBoardRenderer renderer, PuzzleLogger logger)
        : base("move", "Makes a move on the current board.")
    {
        _apiClient = apiClient;
        _solversApiClient = solversApiClient;
        _renderer = renderer;
        _logger = logger;

		var numberOption = new Option<int>(
			aliases: new[] { "--number", "-n" },
			parseArgument: result => {
				if (result.Tokens.Count == 0)
				{
					result.ErrorMessage = "A number to move is required.";
					return default;

				}
				var value = result.Tokens.Single().Value;
				if (!int.TryParse(value, out var number))
				{
					result.ErrorMessage = "A number is required.";
					return default;
				}

				if (number < 1 || number > 15)
				{
					result.ErrorMessage = "The number should be between 1 and 15.";
					return default;
				}

				return number;
			});

        AddOption(numberOption);
        this.SetHandler(Execute, numberOption);
    }

    private async Task Execute(int moveNumber)
    {
		var board = await  _apiClient.Move(moveNumber);
		_renderer.Render(board);
    }
}
