namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public class EpsilonGreedyActionSelectionPolicy
{
    private readonly Random _random;

    public EpsilonGreedyActionSelectionPolicy(Random random)
	{
        _random = random;
    }
}
