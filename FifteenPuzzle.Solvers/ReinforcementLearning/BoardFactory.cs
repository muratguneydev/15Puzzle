namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardFactory
{
	public virtual Board GetRandom() => new RandomBoard();
	public virtual Board Clone(Board board) => new(board);
}