namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public record QLearningSystemConfiguration
{
	public virtual string QValueStorageFilePath { get; set; } = string.Empty;
}