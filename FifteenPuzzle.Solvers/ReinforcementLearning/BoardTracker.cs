using FifteenPuzzle.Game;

namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class BoardTracker
{
	private readonly HashSet<Board> _tracker = new(new BoardComparer());

	public void Add(Board board) => _tracker.Add(board);
	public virtual bool WasProcessedBefore(Board board) => _tracker.Contains(board);

    public void Clear() => _tracker.Clear();
}
