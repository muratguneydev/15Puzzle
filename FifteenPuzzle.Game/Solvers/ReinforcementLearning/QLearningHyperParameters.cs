namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

//Hyperparameters are external configuration variables that data scientists use to manage machine learning model training.
public record QLearningHyperparameters(double LearningRateAlpha,
	double DiscountFactorGamma,
	double ExplorationProbabilityEpsilon,
	int NumberOfIterations);
