namespace FifteenPuzzle.Game;

public class DepthFirstSolver
{
	private readonly Stack<Board> _stack = new();
	private readonly HashSet<Board> _history = new();
    private readonly Action<Board> _onNewItemTested;
	private readonly BoardComparer _boardComparer = new BoardComparer();
    private bool _solved;

    public DepthFirstSolver(Action<Board> onNewItemTested) => _onNewItemTested = onNewItemTested;

    public IReadOnlyList<Board> History => _history.ToList();

	public void Solve(Board initialBoard)
    {
        if (HasBeenTested(initialBoard) || _solved)
            return;

        _onNewItemTested(initialBoard);

        _history.Add(initialBoard);

        if (initialBoard.IsSolved)
        {
            _solved = true;
            return;
        }

        foreach (var nextPossibleBoard in initialBoard.GetFrontierBoards())
        {
            if (!HasBeenTested(nextPossibleBoard))
                _stack.Push(nextPossibleBoard);
        }

        if (!_stack.Any())
            return;

        var nextBoard = _stack.Pop();
        Solve(nextBoard);
    }

    private bool HasBeenTested(Board board) => _history.Contains(board, _boardComparer);
}
