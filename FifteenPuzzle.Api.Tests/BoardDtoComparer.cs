namespace FifteenPuzzle.Api.Tests;

using FifteenPuzzle.Api.Contracts;

public class BoardDtoComparer : IEqualityComparer<BoardDto>
{
    public bool Equals(BoardDto? x, BoardDto? y) =>
		x == null || y == null ? false : Flattened(x.Cells).SequenceEqual(Flattened(y.Cells));
    

    public int GetHashCode(BoardDto board)
	{
		int value=0;
		var cells = board.Cells;
		for (var i = 0;i< cells.Length; i++)
		{
			value = HashCode.Combine(Flattened(cells)[i], value);
		}
		return value;
	}

	public CellDto[] Flattened(CellDto[,] cells)
	{
		var result = new CellDto[cells.Length];

			int flatIndex = 0;
			for (int y = 0; y < cells.GetLength(0); y++)
			{
				for (int x = 0; x < cells.GetLength(1); x++)
				{
					result[flatIndex++] = cells[y, x];
				}
			}
			return result;
	}
}