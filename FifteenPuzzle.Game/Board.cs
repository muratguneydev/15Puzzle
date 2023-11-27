namespace FifteenPuzzle.Game;

using System.Collections;

public class Board : IEnumerable<Row>
{
	public const int SideLength = 4;
	private readonly Cell[,] _cells = new Cell[SideLength,SideLength];

	public Board(string[,] cells)
    {
        AssertBoardBoundaries(cells);

		for (var x = 0; x < SideLength; x++)
        {
            for (var y = 0; y < SideLength; y++)
            {
                _cells[x, y] = new Cell(x, y, cells[x,y]);
            }
        }
    }

	public Board(int[,] cellValues)
		: this(Convert(cellValues))
	{

	}

	public Board(Board board)
    {
		for (var x = 0; x < SideLength; x++)
        {
            for (var y = 0; y < SideLength; y++)
            {
                _cells[x, y] = board.Cells[x,y].Copy();
            }
        }
    }

    public Cell[,] Cells => (Cell[,])_cells.Clone();

	public static Board Solved = new(
		new[,]
		{
			{ "1", "2", "3", "4" },
			{ "5", "6", "7", "8" },
			{ "9", "10", "11", "12" },
			{ "13", "14", "15", "" }
		});

	public IEnumerable<Row> Rows =>
		Enumerable
			.Range(0, RowLength)
            .Select(GetRow)
            .ToArray();

	public void Move(int value)
    {
        var empty = GetCell(string.Empty);
        var from = GetCell(value.ToString());

        var sameColumnAdjacentRow = empty.IsOnSameColumn(from) && Math.Abs(empty.RowDifference(from)) == 1;
        var sameRowAdjacentColumn = empty.IsOnSameRow(from) && Math.Abs(empty.ColumnDifference(from)) == 1;
        if (!sameColumnAdjacentRow && !sameRowAdjacentColumn)
        {
            return;//return invalid move
        }

        _cells[empty.X, empty.Y].SetValue(_cells[from.X, from.Y].Value);
        _cells[from.X, from.Y].SetValue(string.Empty);
    }

    private Cell GetCell(string value)
    {
        for (var x = 0; x < SideLength; x++)
        {
            for (var y = 0; y < SideLength; y++)
            {
                if (_cells[x, y].Value == value)
                {
                    return _cells[x, y];
                }
            }
        }

        throw new Exception("There is no empty cell on the board.");
    }

    public IEnumerator<Row> GetEnumerator() => Rows.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private int ColumnLength => SideLength;
	private int RowLength => SideLength;

    private Row GetRow(int rowIndex) => new Row(
		Enumerable
			.Range(0, ColumnLength)
            .Select(columnIndex => _cells[rowIndex, columnIndex])
            .ToArray());

    private static void AssertBoardBoundaries(string[,] cells)
    {
        AssertBoundary(cells, 0, "row");
        AssertBoundary(cells, 1, "column");
    }

    private static void AssertBoundary(string[,] cells, int dimension, string dimensionName)
    {
        if (cells.GetUpperBound(dimension) != SideLength-1)
        {
            throw new Exception($"Board should have 3 {dimensionName}s.");
        }
    }

	private static string[,] Convert(int[,] cellValues)
	{
		var stringCellValues = new string[SideLength,SideLength];
		for (var x = 0; x < SideLength; x++)
        {
            for (var y = 0; y < SideLength; y++)
            {
                stringCellValues[x, y] = cellValues[x,y] == 0 ? string.Empty : cellValues[x,y].ToString();
            }
        }
		return stringCellValues;
	}
}
