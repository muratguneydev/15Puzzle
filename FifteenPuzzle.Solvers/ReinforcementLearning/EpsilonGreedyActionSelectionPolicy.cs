namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class EpsilonGreedyActionSelectionPolicy : IActionSelectionPolicy
{
    private readonly QLearningHyperparameters _qLearningHyperparameters;
    private readonly Random _random;

    public EpsilonGreedyActionSelectionPolicy(QLearningHyperparameters qLearningHyperparameters, Random random)
    {
        _qLearningHyperparameters = qLearningHyperparameters;
        _random = random;
    }

    public ActionQValue PickAction(BoardActionQValues boardActionQValues)
	{
		var actionQValues = boardActionQValues.ActionQValues.ToArray();

		if (ShouldPickExploration())
		{
			var randomActionIndex = _random.Next(0, actionQValues.Length-1);
			return actionQValues[randomActionIndex];
		}

		return actionQValues.MaxBy(actionQValue => actionQValue.QValue)!;
	}

	private bool ShouldPickExploration()
	{
		var randomProbability = _random.NextDouble();
		return randomProbability < _qLearningHyperparameters.ExplorationProbabilityEpsilon;
	}
}
