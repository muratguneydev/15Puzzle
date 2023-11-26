namespace FifteenPuzzle.Game;

using System.Collections;

public class Board : IEnumerable<Row>
{
	private const int Dimension = 4;
	private readonly int[,] _cells = new int[Dimension,Dimension];

	public Board(int[,] cells)
    {
        AssertBoardBoundaries(cells);
        _cells = cells;
    }

    public int[,] Cells => (int[,])_cells.Clone();

	public static Board Solved = new(
		new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
		});

	public IEnumerable<Row> Rows =>
		Enumerable
			.Range(0, RowLength)
            .Select(GetRow)
            .ToArray();

	public IEnumerator<Row> GetEnumerator() => Rows.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private int ColumnLength => Dimension;
	private int RowLength => Dimension;

    private Row GetRow(int rowIndex) => new Row(
		Enumerable
			.Range(0, ColumnLength)
            .Select(x => _cells[rowIndex, x])
            .ToArray());

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
}
