using System.Collections;

namespace FifteenPuzzle.Game;

public class Board : IEnumerable<Row>
{
	private readonly int[,] _cells = new int[4,4];

	public Board(int[,] cells)
    {
        AssertBoardBoundaries(cells);
        _cells = cells;
    }

    private static void AssertBoardBoundaries(int[,] cells)
    {
        AssertBoundary(cells, 0, "row");
        AssertBoundary(cells, 1, "column");
    }

    private static void AssertBoundary(int[,] cells, int dimension, string dimensionName)
    {
        if (cells.GetUpperBound(dimension) != 3)
        {
            throw new Exception($"Board should have 3 {dimensionName}s.");
        }
    }

    public int[,] Cells => (int[,])_cells.Clone();
	public int ColumnLength => _cells.GetUpperBound(0) + 1;
	public int RowLength => _cells.GetUpperBound(1) + 1;

	public static Board Solved = new(
		new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
		});

	public IEnumerable<Row> Rows
	{
		get
		{
			var rows = new Row[RowLength];
			for (var rowIndex = 0; rowIndex < RowLength; rowIndex++)
			{
				rows[rowIndex] = GetRow(rowIndex);
			}
			return rows;
		}
	}

	private Row GetRow(int rowIndex)
    {
        return new Row(Enumerable.Range(0, _cells.GetLength(1))
                .Select(x => _cells[rowIndex, x])
                .ToArray());
    }

	public IEnumerator<Row> GetEnumerator()
	{

		return Rows.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
