namespace FifteenPuzzle.Solvers;

using FifteenPuzzle.Game;

public class DepthFirstSolver
{
	private readonly Stack<Board> _stack = new();
	private readonly HashSet<Board> _history = new();
    private Action<Board> _onNewItemTested = _ => {};
	private readonly BoardComparer _boardComparer = new();
    private bool _solved;

    //public DepthFirstSolver(Action<Board> onNewItemTested) => _onNewItemTested = onNewItemTested;

    public IReadOnlyList<Board> History => _history.ToList();

	public void AddOnNewItemTested(Action<Board> onNewItemTested) => _onNewItemTested += onNewItemTested;

	public void Solve(Board currentBoard)
    {
        if (HasBeenTested(currentBoard) || _solved)
            return;

        _onNewItemTested(currentBoard);
        MarkAsTested(currentBoard);

        if (currentBoard.IsSolved)
        {
            _solved = true;
            return;
        }

        AddFrontiersToStack(currentBoard);

        if (!_stack.Any())
            return;

        var nextBoard = _stack.Pop();
        Solve(nextBoard);
    }

    private void AddFrontiersToStack(Board currentBoard)
    {
        foreach (var nextPossibleBoard in currentBoard.GetFrontierBoards())
        {
            if (!HasBeenTested(nextPossibleBoard))
                _stack.Push(nextPossibleBoard);
        }
    }

    private void MarkAsTested(Board currentBoard) => _history.Add(currentBoard);

    private bool HasBeenTested(Board board) => _history.Contains(board, _boardComparer);
}
