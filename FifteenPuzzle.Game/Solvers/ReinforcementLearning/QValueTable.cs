namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

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

	public void UpdateQValues(Board board, ActionQValue action, double reward)
	{
		var nextBoard = new Board(board);
		nextBoard.Move(action.Move.Number.ToString());
		var nextActionQValues = Get(nextBoard);

		var maxNextQValue = nextBoard.IsSolved ? 0 : nextActionQValues.MaxQValue;
		var currentQValue = action.QValue;
		var learningRateMultiplier = reward + _qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
		var newCurrentQValue = currentQValue
			+ _qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;

		action.UpdateQValue(newCurrentQValue);
	}

    public IEnumerator<BoardActionQValues> GetEnumerator() => _boardActionQValues.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
