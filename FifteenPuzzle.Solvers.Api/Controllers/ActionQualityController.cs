namespace FifteenPuzzle.Solvers.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using FifteenPuzzle.Solvers.ReinforcementLearning;

[ApiController]
[Route("[controller]")]
public class ActionQualityController : ControllerBase
{
    private readonly QualityValueRepository _qualityValueRepository;

    public ActionQualityController(QualityValueRepository qualityValueRepository)
	{
        _qualityValueRepository = qualityValueRepository;
    }

	[HttpGet("{boardHashCode}")]
    public async Task<IActionResult> Get(int boardHashCode, CancellationToken cancellationToken)
    {
        //var board = GetBoard(boardDto);
		
        return Ok(new ActionQValue[] {});
    }

	// private Board GetBoard(BoardDto boardDto)
    // {
	// 	var boardCells = boardDto.Cells;
    //     var cells = new Cell[Board.SideLength,Board.SideLength];
	// 	for (var row = 0; row < Board.SideLength; row++)
    //     {
    //         for (var column = 0; column < Board.SideLength; column++)
    //         {
    //             cells[row, column] = new Cell(row, column, boardCells[row,column].Value);
    //         }
    //     }
	// 	return new (cells);
    // }
}


