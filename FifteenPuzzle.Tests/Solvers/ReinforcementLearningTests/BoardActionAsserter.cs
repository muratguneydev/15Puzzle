namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Diagnostics.CodeAnalysis;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using Shouldly;

public class BoardActionAsserter
{
    public static void ShouldBeEquivalent(BoardAction expected, BoardAction actual)
    {
		// BoardAsserter.ShouldBeEquivalent(expected.Board, actual.Board);
		// BoardAsserter.ShouldBeEquivalent(expected.NextBoard, actual.NextBoard);
		// actual.ActionQValue.ShouldBe(expected.ActionQValue);
        new BoardActionComparer().Equals(expected, actual).ShouldBeTrue();
    }
}

public class BoardActionComparer : IEqualityComparer<BoardAction>
{
	private const int ForceEquals = default;

    public bool Equals(BoardAction? x, BoardAction? y) =>
        x == null || y == null ? false : AreBoardActionsEqual(x, y);

    private static bool AreBoardActionsEqual(BoardAction x, BoardAction y) =>
		new BoardComparer().Equals(x.Board, y.Board) && x.ActionQValue.Equals(y.ActionQValue);

    public int GetHashCode(BoardAction boardAction) => ForceEquals;
}

public class ActionQValuesComparer : IEqualityComparer<ActionQValues>
{
	private const int ForceEquals = default;

    public bool Equals(ActionQValues? x, ActionQValues? y) =>
        x == null || y == null ? false : Order(x).SequenceEqual(Order(y), new ActionQValueComparer());

    private static IOrderedEnumerable<ActionQValue> Order(ActionQValues x) => x.OrderBy(a => a.Move.Number);

    public int GetHashCode([DisallowNull] ActionQValues obj) => ForceEquals;
}

public class ActionQValueComparer : IEqualityComparer<ActionQValue>
{
	private const int ForceEquals = default;

    public bool Equals(ActionQValue? x, ActionQValue? y) =>
        x == null || y == null ? false : AreEqual(x, y);

    public int GetHashCode([DisallowNull] ActionQValue obj) => ForceEquals;

    private bool AreEqual(ActionQValue x, ActionQValue y) => x.Move.Equals(y.Move) && x.QValue == y.QValue;
}

public class BoardActionQValuesComparer : IEqualityComparer<BoardActionQValues>
{
	private const int ForceEquals = default;

    public bool Equals(BoardActionQValues? x, BoardActionQValues? y) =>
        x == null || y == null
			? false
			: new BoardComparer().Equals(x.Board, y.Board)
				&& new ActionQValuesComparer().Equals(x.ActionQValues, y.ActionQValues);


    public int GetHashCode([DisallowNull] BoardActionQValues obj) => ForceEquals;
}

public class QValueTableComparer : IEqualityComparer<QValueTable>
{
	private const int ForceEquals = default;
	private static readonly BoardComparer _boardComparer = new();
	private static readonly BoardActionQValuesComparer _boardActionQValuesComparer = new();

    public bool Equals(QValueTable? x, QValueTable? y) =>
        x == null || y == null
			? false
			: Order(x).SequenceEqual(Order(y), _boardActionQValuesComparer);

    private static IOrderedEnumerable<BoardActionQValues> Order(QValueTable x) =>
		x.OrderBy(qValueTable => _boardComparer.GetHashCode(qValueTable.Board));


    public int GetHashCode([DisallowNull] QValueTable obj) => ForceEquals;
}
