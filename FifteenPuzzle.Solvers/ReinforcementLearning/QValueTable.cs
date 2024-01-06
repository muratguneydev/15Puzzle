namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Collections;
using FifteenPuzzle.Game;

public record QValueTable : IEnumerable<BoardActionQValues>
{
	private readonly Dictionary<int,BoardActionQValues> _boardActionQValues;
	private static readonly BoardComparer _boardComparer = new();
    private readonly QLearningHyperparameters _qLearningHyperparameters;

    public QValueTable(IEnumerable<BoardActionQValues> boardActionQValues, QLearningHyperparameters qLearningHyperparameters)
    {
        _boardActionQValues = boardActionQValues.ToDictionary(b => _boardComparer.GetHashCode(b.Board));
        _qLearningHyperparameters = qLearningHyperparameters;
    }

    public ActionQValues Get(Board board)
	{
		if (_boardActionQValues.TryGetValue(_boardComparer.GetHashCode(board), out var boardActionQValues))
		{
			return boardActionQValues.ActionQValues;
		}

		return ActionQValues.Empty;
	}

	public void UpdateQValues(BoardAction boardAction, double reward)
	{
		// var nextBoard = new Board(board);
		// nextBoard.Move(action.Move.Number.ToString());
		var nextActionQValues = Get(boardAction.NextBoard);

		var maxNextQValue = boardAction.NextBoard.IsSolved ? 0 : nextActionQValues.MaxQValue;
		var currentQValue = boardAction.ActionQValue.QValue;
		var learningRateMultiplier = reward + _qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
		var newCurrentQValue = currentQValue
			+ _qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;

		boardAction.ActionQValue.UpdateQValue(newCurrentQValue);
	}

	public static QValueTable Empty(QLearningHyperparameters qLearningHyperparameters) =>
		new QValueTable(Enumerable.Empty<BoardActionQValues>(), qLearningHyperparameters);

    public IEnumerator<BoardActionQValues> GetEnumerator() => _boardActionQValues.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
