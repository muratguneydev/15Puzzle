namespace FifteenPuzzle.Game;

public class BoardComparer : IEqualityComparer<Board>
{
    public bool Equals(Board? x, Board? y) =>
		x == null || y == null ? false : x.Flattened.SequenceEqual(y.Flattened);
    

    public int GetHashCode(Board board)
	{
		int value=0;
		var cells = board.Flattened;
		for (var i = 0;i< cells.Length; i++)
		{
			value = HashCode.Combine(cells[i], value);
		}
		return value;
	}
}