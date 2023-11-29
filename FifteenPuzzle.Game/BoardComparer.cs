namespace FifteenPuzzle.Game;

public class BoardComparer : IEqualityComparer<Board>
{
    public bool Equals(Board? x, Board? y) =>
		x == null || y == null ? false : x.Flattened.SequenceEqual(y.Flattened);
    

    public int GetHashCode(Board board) => board.GetHashCode();
}