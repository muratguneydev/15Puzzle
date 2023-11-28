namespace FifteenPuzzle.Game;

using System.Collections;
using System.Text;

public class Board : IEnumerable<Row>
{
	public const int SideLength = 4;
	private readonly Cell[,] _cells = new Cell[SideLength,SideLength];

	public Board(string[,] cells)
    {
        AssertBoardBoundaries(cells);

		for (var row = 0; row < RowLength; row++)
        {
            for (var column = 0; column < ColumnLength; column++)
            {
                _cells[row, column] = new Cell(row, column, cells[row,column]);
            }
        }
    }

	public Board(int[,] cellValues)
		: this(Convert(cellValues))
	{

	}

	public Board(Board board)
    {
		for (var row = 0; row < RowLength; row++)
        {
            for (var column = 0; column < ColumnLength; column++)
            {
                _cells[row, column] = board.Cells[row,column].Copy();
            }
        }
    }

    public Cell[,] Cells
	{
		get
		{
			var cells = new Cell[SideLength,SideLength];
			for (var row = 0; row < RowLength; row++)
			{
				for (var column = 0; column < ColumnLength; column++)
				{
					cells[row, column] = _cells[row,column].Copy();
				}
			}
			return cells;
		}
	}

	public Cell[] Flattened
	{
		get
		{
			var result = new Cell[_cells.Length];

			int write = 0;
			for (int y = 0; y < RowLength; y++)
			{
				for (int x = 0; x < ColumnLength; x++)
				{
					result[write++] = _cells[y, x];
				}
			}
			return result;
		}
	}

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

	public void Move(string value)
    {
        Cell empty = GetEmptyCell();
        var from = GetCell(value);

        var sameColumnAdjacentRow = empty.IsOnSameColumn(from) && Math.Abs(empty.RowDifference(from)) == 1;
        var sameRowAdjacentColumn = empty.IsOnSameRow(from) && Math.Abs(empty.ColumnDifference(from)) == 1;
        if (!sameColumnAdjacentRow && !sameRowAdjacentColumn)
        {
            return;//return invalid move
        }

        _cells[empty.Row, empty.Column].SetValue(_cells[from.Row, from.Column].Value);
        _cells[from.Row, from.Column].SetValue(string.Empty);
    }

    public IEnumerable<Board> GetFrontierBoards()
	{
		var adjacentCells = GetAdjacentCells(GetEmptyCell());
		var result = new List<Board>();

		foreach(var cell in adjacentCells)
		{
			var clone = new Board(this);
			clone.Move(cell.Value);
			result.Add(clone);
		}

		return result;
	}

	public IEnumerator<Row> GetEnumerator() => Rows.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        var stringBuilder = new StringBuilder(_cells.Length);
		for (var row = 0; row < RowLength; row++)
		{
			for (var column = 0; column < ColumnLength; column++)
			{
				stringBuilder.Append($"{_cells[row,column]}-");
			}
		}
		return stringBuilder.ToString();
    }

    private IEnumerable<Cell> GetAdjacentCells(Cell cell)
	{
		var adjacentCells = new List<Cell>();
		if (TryGetCell(cell.Row - 1, cell.Column, out var aboveCell))
		{
			adjacentCells.Add(aboveCell);
		}
		if (TryGetCell(cell.Row + 1, cell.Column, out var belowCell))
		{
			adjacentCells.Add(belowCell);
		}
		if (TryGetCell(cell.Row, cell.Column - 1, out var leftCell))
		{
			adjacentCells.Add(leftCell);
		}
		if (TryGetCell(cell.Row, cell.Column + 1, out var rightCell))
		{
			adjacentCells.Add(rightCell);
		}

		return adjacentCells;
	}

    private Cell GetEmptyCell() => GetCell(string.Empty);

	private Cell GetCell(string value)
    {
        for (var row = 0; row < RowLength; row++)
        {
            for (var column = 0; column <  ColumnLength; column++)
            {
                if (_cells[row, column].Value == value)
                {
                    return _cells[row, column];
                }
            }
        }

        throw new Exception("There is no empty cell on the board.");
    }

	private bool TryGetCell(int row, int column, out Cell cell)
	{
		cell = new Cell(0,0,"");//TODO
		if (row < 0)
			return false;
		if (row >= SideLength)
			return false;
		if (column < 0)
			return false;
		if (column >= SideLength)
			return false;
		
		cell = _cells[row,column];
		return true;
	}

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
		for (var row = 0; row < SideLength; row++)
        {
            for (var column = 0; column < SideLength; column++)
            {
                stringCellValues[row, column] = cellValues[row,column] == 0 ? string.Empty : cellValues[row,column].ToString();
            }
        }
		return stringCellValues;
	}
}
