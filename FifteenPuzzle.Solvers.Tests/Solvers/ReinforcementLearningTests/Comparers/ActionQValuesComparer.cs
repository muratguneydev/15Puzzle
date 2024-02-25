namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Diagnostics.CodeAnalysis;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class ActionQValuesComparer : IEqualityComparer<ActionQValues>
{
	private const int ForceEquals = default;

    public bool Equals(ActionQValues? x, ActionQValues? y) =>
        x == null || y == null ? false : Order(x).SequenceEqual(Order(y), new ActionQValueComparer());

    private static IOrderedEnumerable<ActionQValue> Order(ActionQValues x) => x.OrderBy(a => a.Move.Number);

    public int GetHashCode([DisallowNull] ActionQValues obj) => ForceEquals;
}
