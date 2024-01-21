namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class QValueCalculator
{
    private readonly QLearningHyperparameters _qLearningHyperparameters;

    public QValueCalculator(QLearningHyperparameters qLearningHyperparameters) =>
		_qLearningHyperparameters = qLearningHyperparameters;

    public virtual double Calculate(BoardAction boardAction, ActionQValues nextActionQValues, double reward)
	{
		var maxNextQValue = boardAction.NextBoard.IsSolved ? 0 : nextActionQValues.MaxQValue;
		var currentQValue = boardAction.ActionQValue.QValue;
		var learningRateMultiplier = reward + _qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
		var newCurrentQValue = currentQValue
			+ _qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;
		return newCurrentQValue;
	}
}