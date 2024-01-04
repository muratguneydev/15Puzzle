namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public class EpsilonGreedyActionSelectionPolicy
{
    private readonly Random _random;

    public EpsilonGreedyActionSelectionPolicy(Random random) => _random = random;

    public virtual ActionQValue PickAction(BoardActionQValues boardActionQValues, double explorationProbabilityEpsilon)
	{
		var actionQValues = boardActionQValues.ActionQValues.ToArray();

		if (ShouldPickExploration(explorationProbabilityEpsilon))
		{
			var randomActionIndex = _random.Next(0, actionQValues.Length-1);
			return actionQValues[randomActionIndex];
		}

		return actionQValues.MaxBy(actionQValue => actionQValue.QValue)!;
	}

	private bool ShouldPickExploration(double explorationProbabilityEpsilon)
	{
		var randomProbability = _random.NextDouble();
		return randomProbability < explorationProbabilityEpsilon;
	}
}
