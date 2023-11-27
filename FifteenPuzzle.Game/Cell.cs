namespace FifteenPuzzle.Game;

public record Cell
{
	private static readonly HashSet<string> PossibleValues = new[] { "", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" }.ToHashSet();
	
	public Cell(int x, int y, string value)
    {
        AssertBoundary(x, "X");
        AssertBoundary(y, "Y");
        AssertValue(value);

        X = x;
        Y = y;
        Value = value;
    }

    public int X { get; }
    public int Y { get; }
    public string Value { get; private set; }

    public bool IsOnSameRow(Cell other) => Y == other.Y;
	public bool IsOnSameColumn(Cell other) => X == other.X;
	public int RowDifference(Cell other) => Y - other.Y;
	public int ColumnDifference(Cell other) => X - other.X;
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