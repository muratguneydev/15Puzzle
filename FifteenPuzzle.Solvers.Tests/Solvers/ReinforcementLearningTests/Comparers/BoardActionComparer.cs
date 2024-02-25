namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class BoardActionComparer : IEqualityComparer<BoardAction>
{
	private const int ForceEquals = default;

    public bool Equals(BoardAction? x, BoardAction? y) =>
        x == null || y == null ? false : AreBoardActionsEqual(x, y);

    private static bool AreBoardActionsEqual(BoardAction x, BoardAction y) =>
		new BoardComparer().Equals(x.Board, y.Board) && x.ActionQValue.Equals(y.ActionQValue);

    public int GetHashCode(BoardAction boardAction) => ForceEquals;
}
