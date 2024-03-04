namespace FifteenPuzzle.Tests.Common;

using System.Diagnostics.CodeAnalysis;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class ActionQValueComparer : IEqualityComparer<ActionQValue>
{
	private const int ForceEquals = default;

    public bool Equals(ActionQValue? x, ActionQValue? y) =>
        x == null || y == null ? false : AreEqual(x, y);

    public int GetHashCode([DisallowNull] ActionQValue obj) => ForceEquals;

    private bool AreEqual(ActionQValue x, ActionQValue y) => x.Move.Equals(y.Move) && x.QValue == y.QValue;
}
