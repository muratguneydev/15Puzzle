namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Diagnostics.CodeAnalysis;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common;

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
