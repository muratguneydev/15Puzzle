namespace FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;

public class EpsilonGreedyExploreActionSelectionPolicy : IActionSelectionPolicy
{
    private readonly Random _random;

    public EpsilonGreedyExploreActionSelectionPolicy(Random random) => _random = random;

    public ActionQValue PickAction(ActionQValues actionQValueCollection)
	{
		var actionQValues = actionQValueCollection.ToArray();
		var randomActionIndex = _random.Next(0, actionQValues.Length-1);
		return actionQValues[randomActionIndex];
	}
}
