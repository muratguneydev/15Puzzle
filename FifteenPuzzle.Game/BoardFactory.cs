namespace FifteenPuzzle.Game;

public class BoardFactory
{
	public virtual Board GetRandom() => new RandomBoard();
	public virtual Board GetSolved() => Board.Solved;
	public virtual Board Clone(Board board) => new(board);

	/// <summary>
	/// The 15 puzzle does not always have a solution.
	/// The solvability of a 15 puzzle depends on the initial configuration of the tiles.
	/// Specifically, it depends on the parity (even or odd) of the permutation of the tiles.
	/// Here's a rule to determine whether a given configuration of the 15 puzzle is solvable:
	/// Even Permutation: If the number of inversions (pairs of tiles that are out of order) is even, the puzzle is solvable.
	/// Odd Permutation: If the number of inversions is odd, the puzzle is unsolvable.
	/// Inversions are counted by comparing each tile with every tile that comes after it.
	/// If a tile has a lower number than a tile following it, it is considered an inversion.
	/// The empty space is also considered as a tile with the number 0.
	/// This rule is derived from the fact that a single move (a swap) changes the parity of the number of inversions.
	/// So, if the initial configuration has an even number of inversions, it is always possible to reach the solved state with a series of legal moves, and the puzzle is solvable.
	/// Keep in mind that if the 15 puzzle has an odd number of inversions, no sequence of legal moves can reach the solved state, and the puzzle is unsolvable. In such cases, you would need to shuffle the tiles again to get a solvable configuration.
	/// </summary>
	public virtual Board GetSolvable()
	{
		const int MaxNumberOfTries = 20;
		var numberOfTries = 0;
		Board board;
		do
		{
			board = GetRandom();
			numberOfTries++;
		}
		while (!board.IsSolvable && numberOfTries < MaxNumberOfTries);

		if (!board.IsSolvable)
			throw new Exception($"Couldn't find a solvable board in {numberOfTries} tries.");

		return board;
	}
}
