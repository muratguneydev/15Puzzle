namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public interface IRewardStrategy
{
	double Calculate(Board board);
}
