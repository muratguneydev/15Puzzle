namespace FifteenPuzzle.Solvers.ReinforcementLearning;

//Hyperparameters are external configuration variables that data scientists use to manage machine learning model training.
public class QLearningHyperparameters
{
    public QLearningHyperparameters(double learningRateAlpha,
		double discountFactorGamma,
		double explorationProbabilityEpsilon,
		int numberOfIterations)
	{
        LearningRateAlpha = learningRateAlpha;
        DiscountFactorGamma = discountFactorGamma;
        ExplorationProbabilityEpsilon = explorationProbabilityEpsilon;
        NumberOfIterations = numberOfIterations;
    }

    public double LearningRateAlpha { get; }
    public double DiscountFactorGamma { get; }
    public virtual double ExplorationProbabilityEpsilon { get; }
    public virtual int NumberOfIterations { get; }
}
