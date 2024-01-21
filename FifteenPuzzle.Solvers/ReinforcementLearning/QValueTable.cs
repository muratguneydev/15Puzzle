namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Collections;
using FifteenPuzzle.Game;

public record QValueTable : IEnumerable<BoardActionQValues>
{
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
		// var nextActionQValues = GetOrAddDefaultActions(boardAction.NextBoard);

		// var maxNextQValue = boardAction.NextBoard.IsSolved ? 0 : nextActionQValues.MaxQValue;
		// var currentQValue = boardAction.ActionQValue.QValue;
		// var learningRateMultiplier = reward + _qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
		// var newCurrentQValue = currentQValue
		// 	+ _qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;

		var actionToUpdate = GetOrAddDefaultActions(boardAction.Board).Get(boardAction.ActionQValue);
		actionToUpdate.UpdateQValue(reward);
	}

    // public void UpdateQValues(BoardAction boardAction, double reward)
	// {
	// 	var nextActionQValues = GetOrAddDefaultActions(boardAction.NextBoard);

	// 	var maxNextQValue = boardAction.NextBoard.IsSolved ? 0 : nextActionQValues.MaxQValue;
	// 	var currentQValue = boardAction.ActionQValue.QValue;
	// 	var learningRateMultiplier = reward + _qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
	// 	var newCurrentQValue = currentQValue
	// 		+ _qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;

	// 	var actionToUpdate = GetOrAddDefaultActions(boardAction.Board).Get(boardAction.ActionQValue);
	// 	actionToUpdate.UpdateQValue(newCurrentQValue);
	// }

	public static QValueTable Empty(QLearningHyperparameters qLearningHyperparameters) =>
		new (Enumerable.Empty<BoardActionQValues>());

    public IEnumerator<BoardActionQValues> GetEnumerator() => _boardActionQValues.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static ActionQValues GetDefaultActionQValues(Board board) =>
		new (board.GetMoves().Select(move => new ActionQValue(move, 0)));
}
