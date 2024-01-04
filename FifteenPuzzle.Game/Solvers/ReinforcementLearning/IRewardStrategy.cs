namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public interface IRewardStrategy
{
	double Calculate(Board board);
}
