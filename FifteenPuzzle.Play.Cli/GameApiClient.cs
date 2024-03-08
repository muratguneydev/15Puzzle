namespace FifteenPuzzle.Play.Cli;

using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using Newtonsoft.Json;

public class GameApiClient
{
	public const string Name = "ApiHttpClient";
    private readonly IHttpClientFactory _httpClientFactory;

    public GameApiClient(IHttpClientFactory httpClientFactory)
	{
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Board> GetCurrentBoard()
    {
        var httpClient = _httpClientFactory.CreateClient(Name);
		var response = await httpClient.GetStringAsync("/Game");
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(response)
            ?? throw new Exception("Deserialized board is null.");
		var board = GetBoard(gameState.Board);
		return board;
    }

	public async Task<Board> GetNewBoard()
    {
        var httpClient = _httpClientFactory.CreateClient(Name);
		var response = await httpClient.PutAsync("/Game/new", null);
		var responseString = await response.Content.ReadAsStringAsync();
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(responseString)
            ?? throw new Exception("Deserialized board is null.");
		var board = GetBoard(gameState.Board);
		return board;
    }

	//TODO: method for getting data from ActionQualityController

	private Board GetBoard(BoardDto boardDto)
    {
		var boardCells = boardDto.Cells;
        var cells = new Cell[Board.SideLength,Board.SideLength];
		for (var row = 0; row < Board.SideLength; row++)
        {
            for (var column = 0; column < Board.SideLength; column++)
            {
                cells[row, column] = new Cell(row, column, boardCells[row,column].Value);
            }
        }
		return new (cells);
    }
}
