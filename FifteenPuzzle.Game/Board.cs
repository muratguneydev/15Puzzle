using System.Collections;

namespace FifteenPuzzle.Game;

public class Board : IEnumerable<int[]>
{
	private int[,] _cells = new int[4, 4];

	public Board(int[,] cells)
	{
		if (cells.GetUpperBound(0) != 3)
		{
			throw new Exception("Board should have 3 rows.");
		}

		if (cells.GetUpperBound(1) != 3)
		{
			throw new Exception("Board should have 3 columns.");
		}

		_cells = cells;
	}

	public int[,] Cells => (int[,])_cells.Clone();
	public int ColumnLength => _cells.GetUpperBound(0) + 1;
	public int RowLength => _cells.GetUpperBound(1) + 1;

	public static Board Complete = new(
		new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
		});

	public IEnumerable<int[]> Rows
	{
		get
		{
			var rows = new int[RowLength][];
			for (var row = 0; row < RowLength; row++)
			{
				rows[row] = new int[ColumnLength];
				for (var column = 0; column < ColumnLength; column++)
				{
					rows[row][column] = _cells[row, column];
				}
			}
			return rows;
		}
	}

	public IEnumerator<int[]> GetEnumerator()
	{

		return Rows.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}