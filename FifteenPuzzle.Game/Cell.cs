namespace FifteenPuzzle.Game;

public record Cell
{
	private static readonly HashSet<string> PossibleValues = new[] { "", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" }.ToHashSet();
	
	public Cell(int row, int column, string value)
    {
        AssertBoundary(row, "Row");
        AssertBoundary(column, "Column");
        AssertValue(value);

        Row = row;
        Column = column;
        Value = value;
    }

    public int Row { get; }
    public int Column { get; }
    public string Value { get; private set; }
    public int NumberValue => string.IsNullOrEmpty(Value) ? 0 : int.Parse(Value);

    public bool IsOnSameRow(Cell other) => Column == other.Column;
	public bool IsOnSameColumn(Cell other) => Row == other.Row;
	public int RowDifference(Cell other) => Column - other.Column;
	public int ColumnDifference(Cell other) => Row - other.Row;
	public void SetValue(string value) => Value = value;
	public Cell Copy() => this with {};

    private static void AssertBoundary(int axis, string axisName)
    {
        if (axis < 0 || axis >= Board.SideLength)
        {
            throw new Exception($"Cell {axisName}:{axis} is outside of the boundaries.");
        }
    }

	private static void AssertValue(string value)
    {
        if (!PossibleValues.Contains(value))
        {
            throw new Exception($"Invalid cell value {value} provided.");
        }
    }
}