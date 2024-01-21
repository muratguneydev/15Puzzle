namespace FifteenPuzzle.Solvers.ReinforcementLearning;

//Hyperparameters are external configuration variables that data scientists use to manage machine learning model training.
public record QLearningHyperparameters
{
    public virtual double LearningRateAlpha { get; set; }
    public virtual double DiscountFactorGamma { get; set; }
    public virtual double ExplorationProbabilityEpsilon { get; set; }
    public virtual int NumberOfIterations { get; set; }
}
