namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Collections;
using System.Diagnostics;
using FifteenPuzzle.Game;

public record QValueTable : IEnumerable<BoardActionQValues>
{
	public const double DefaultQValue = 0;

	[DebuggerDisplay("{ToString()}")]
	private readonly Dictionary<int,BoardActionQValues> _boardActionQValues;
	private static readonly BoardComparer _boardComparer = new();

    public QValueTable(IEnumerable<BoardActionQValues> boardActionQValues) =>
		_boardActionQValues = boardActionQValues.ToDictionary(b => _boardComparer.GetHashCode(b.Board));

    public ActionQValues GetOrAddDefaultActions(Board board)
    {
        if (_boardActionQValues.TryGetValue(_boardComparer.GetHashCode(board), out var boardActionQValues))
        {
            return boardActionQValues.ActionQValues;
        }

        var defaultActions = GetDefaultActionQValues(board);
		_boardActionQValues.Add(_boardComparer.GetHashCode(board), new BoardActionQValues(board, defaultActions));
		return defaultActions;
    }

	public void UpdateQValues(BoardAction boardAction, double reward)
	{
		var actionToUpdate = GetOrAddDefaultActions(boardAction.Board).Get(boardAction.ActionQValue);
		actionToUpdate.UpdateQValue(reward);
	}

    public static QValueTable Empty =>
		new (Enumerable.Empty<BoardActionQValues>());

    public IEnumerator<BoardActionQValues> GetEnumerator() => _boardActionQValues.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public sealed override string ToString()
		=> string.Join(Environment.NewLine, this.Select(actions => actions.ToString()));

    private static ActionQValues GetDefaultActionQValues(Board board) =>
		new (board.GetMoves().Select(move => new ActionQValue(move, DefaultQValue)));
}
