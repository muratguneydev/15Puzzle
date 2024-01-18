namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardMoveComparer : IEqualityComparer<BoardMove>
{
	private static BoardComparer BoardComparer = new();
    public bool Equals(BoardMove? x, BoardMove? y) =>
		x == null || y == null ? false : BoardComparer.Equals(x.Board, y.Board) && x.Move.Equals(y.Move);

    public int GetHashCode(BoardMove boardMove) =>
		HashCode.Combine(BoardComparer.GetHashCode(boardMove.Board), boardMove.Move);
}