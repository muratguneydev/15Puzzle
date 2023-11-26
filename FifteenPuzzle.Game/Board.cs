namespace FifteenPuzzle.Game;

using System.Collections;

public class Board : IEnumerable<Row>
{
	public const int SideLength = 4;
	private readonly int[,] _cells = new int[SideLength,SideLength];

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

	public void Move(Cell from)
	{
		var empty = new Cell(0,0);
		for (var x=0;x < SideLength;x++)
		{
			for (var y=0;y < SideLength;y++)
			{
				if (_cells[x,y] == 0)
				{
					empty = new Cell(x,y);
					break;
				}
			}
		}

		var sameColumnAdjacentRow = empty.IsOnSameColumn(from) && Math.Abs(empty.RowDifference(from)) == 1;
		var sameRowAdjacentColumn = empty.IsOnSameRow(from) && Math.Abs(empty.ColumnDifference(from)) == 1;
		if (!sameColumnAdjacentRow && !sameRowAdjacentColumn)
		{
			return;
		}
		
		_cells[empty.X, empty.Y] = _cells[from.X, from.Y];
		_cells[from.X, from.Y] = 0;
	}

	public IEnumerator<Row> GetEnumerator() => Rows.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private int ColumnLength => SideLength;
	private int RowLength => SideLength;

    private Row GetRow(int rowIndex) => new Row(
		Enumerable
			.Range(0, ColumnLength)
            .Select(columnIndex => _cells[rowIndex, columnIndex])
            //.Select(columnIndex => _cells[columnIndex, rowIndex])
            .ToArray());

    private static void AssertBoardBoundaries(int[,] cells)
    {
        AssertBoundary(cells, 0, "row");
        AssertBoundary(cells, 1, "column");
    }

    private static void AssertBoundary(int[,] cells, int dimension, string dimensionName)
    {
        if (cells.GetUpperBound(dimension) != SideLength-1)
        {
            throw new Exception($"Board should have 3 {dimensionName}s.");
        }
    }
}

public record Cell
{
	public Cell(int x, int y)
	{
		AssertBoundary(x, "X");
		AssertBoundary(y, "Y");
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

	public bool IsOnSameRow(Cell other) => Y == other.Y;
	public bool IsOnSameColumn(Cell other) => X == other.X;
	public int RowDifference(Cell other) => Y - other.Y;
	public int ColumnDifference(Cell other) => X - other.X;
	

	private static void AssertBoundary(int axis, string axisName)
    {
        if (axis < 0 || axis >= Board.SideLength)
        {
            throw new Exception($"Cell {axisName}:{axis} is outside of the boundaries.");
        }
    }
}