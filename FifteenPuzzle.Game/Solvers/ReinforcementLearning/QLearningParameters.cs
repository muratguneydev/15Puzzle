namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public record QLearningParameters(double LearningRateAlpha,
	double DiscountFactorGamma,
	double ExplorationProbabilityEpsilon,
	int NumberOfIterations);
