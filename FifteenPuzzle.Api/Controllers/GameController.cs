namespace FifteenPuzzle.Api.Controllers;

using System;
using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly BoardStorage _boardStorage;

    public GameController(BoardStorage boardStorage)
	{
        _boardStorage = boardStorage;
    }

	[HttpGet]
    public async Task<IActionResult> GetGameState(CancellationToken cancellationToken)
    {
        var board = await _boardStorage.Get(cancellationToken);
		var boardDto = GetBoardDto(board);
		var gameState = new GameStateDto(boardDto);
        //return Ok(gameState);
        return Ok();
    }

    [HttpPost]
    public IActionResult UpdateGameState([FromBody] string newGameState)
    {
        
        return Ok();
    }

	[HttpPut]
	public async Task<IActionResult> Move(int number)
	{
		var jsonData = HttpContext.Session.GetString("Board");

		if (jsonData == null)
		{
			return NotFound("No existing game session found.");
		}

		var board = await new StringContent(jsonData).ReadFromJsonAsync<Board>();

		//await JsonContent.ReadFromJsonAsync<Board>(jsonData);

		//JsonConvert.DeserializeObject<YourComplexObject>(jsonData);


		return Ok("Game data loaded from session.");
	}

	private BoardDto GetBoardDto(Board board)
    {
		var boardCells = board.Cells;
        var cellDtos = new CellDto[Board.SideLength,Board.SideLength];
		for (var row = 0; row < Board.SideLength; row++)
        {
            for (var column = 0; column < Board.SideLength; column++)
            {
                cellDtos[row, column] = new CellDto(row, column, boardCells[row,column].Value);
            }
        }
		return new (cellDtos);
    }
}
