namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearningSystemConfiguration
{
	public QLearningSystemConfiguration(string qValueStorageFilePath)
	{
        QValueStorageFilePath = qValueStorageFilePath;
    }

    public virtual string QValueStorageFilePath { get; }
}