namespace FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;

public class EpsilonGreedyActionSelectionPolicyFactory : IActionSelectionPolicyFactory
{
    private readonly QLearningHyperparameters _qLearningHyperparameters;
    private readonly Random _random;

    public EpsilonGreedyActionSelectionPolicyFactory(
		QLearningHyperparameters qLearningHyperparameters,
		Random random)
	{
        _qLearningHyperparameters = qLearningHyperparameters;
        _random = random;
    }

	public IActionSelectionPolicy Get()
	{
		return ShouldPickExploration()
			? new EpsilonGreedyExploreActionSelectionPolicy(_random)
			: new EpsilonGreedyExploitActionSelectionPolicy();

	}

	private bool ShouldPickExploration()
	{
		var randomProbability = _random.NextDouble();
		return randomProbability < _qLearningHyperparameters.ExplorationProbabilityEpsilon;
	}
}
