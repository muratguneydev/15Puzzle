namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class BoardMoveTracker
{
	private readonly HashSet<BoardMove> _tracker = new(new BoardMoveComparer());

	public void Add(BoardMove boardMove) => _tracker.Add(boardMove);
	public virtual bool WasProcessedBefore(BoardMove boardMove) => _tracker.Contains(boardMove);

    public void Clear() => _tracker.Clear();
}
