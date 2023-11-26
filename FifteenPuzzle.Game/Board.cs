using System.Collections;

namespace FifteenPuzzle.Game;

public class Board : IEnumerable<Row>
{
	private const int Dimension = 4;
	private readonly int[,] _cells = new int[Dimension,Dimension];

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
        if (cells.GetUpperBound(dimension) != Dimension-1)
        {
            throw new Exception($"Board should have 3 {dimensionName}s.");
        }
    }

    public int[,] Cells => (int[,])_cells.Clone();
	public int ColumnLength => Dimension;
	public int RowLength => Dimension;

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
        return new Row(Enumerable.Range(0, ColumnLength-1)
                .Select(x => _cells[rowIndex, x])
                .ToArray());
    }

    public IEnumerator<Row> GetEnumerator() => Rows.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
