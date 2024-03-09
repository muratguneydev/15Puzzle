namespace FifteenPuzzle.Api.Controllers;

using System.Globalization;
using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly BoardSessionRepository _boardSessionRepository;

    public GameController(BoardSessionRepository boardSessionRepository)
	{
        _boardSessionRepository = boardSessionRepository;
    }

	[HttpGet]
    public async Task<IActionResult> GetGameState(CancellationToken cancellationToken)
    {
        var board = await _boardSessionRepository.Get(cancellationToken);
		var boardDto = GetBoardDto(board);
		var gameState = new GameStateDto(boardDto);
        return Ok(gameState);
    }

	[HttpPut("new")]
	public async Task<IActionResult> NewGame(CancellationToken cancellationToken)
	{
		var board = new RandomBoard();
		await _boardSessionRepository.Update(board, cancellationToken);

		var boardDto = GetBoardDto(board);
		var gameState = new GameStateDto(boardDto);
        return Ok(gameState);
	}

    // [HttpPut]
    // //[HttpPut("{number}")]
	// public async Task<IActionResult> Move([FromBody] int number, CancellationToken cancellationToken)
	// {
	// 	var board = await _boardStorage.Get(cancellationToken);
	// 	board.Move(number.ToString());

	// 	var boardDto = GetBoardDto(board);
	// 	var gameState = new GameStateDto(boardDto);
    //     return Ok(gameState);
	// }

    [HttpPut("{number}")]
	public async Task<IActionResult> Move(int number, CancellationToken cancellationToken)
	{
		var board = await _boardSessionRepository.Get(cancellationToken);
		board.Move(number.ToString());

		await _boardSessionRepository.Update(board, cancellationToken);

		var boardDto = GetBoardDto(board);
		var gameState = new GameStateDto(boardDto);
        return Ok(gameState);
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
