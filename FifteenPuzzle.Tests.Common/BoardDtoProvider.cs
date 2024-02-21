namespace FifteenPuzzle.Tests.Common;

using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;

public static class BoardDtoProvider
{
	public static BoardDto Get(Board board)
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
