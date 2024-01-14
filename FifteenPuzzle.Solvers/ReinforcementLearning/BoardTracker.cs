namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardTracker
{
	private readonly HashSet<Board> _tracker = new(new BoardComparer());

	public void Add(Board board) => _tracker.Add(board);
	public virtual bool WasProcessedBefore(Board board) => _tracker.Contains(board);
}