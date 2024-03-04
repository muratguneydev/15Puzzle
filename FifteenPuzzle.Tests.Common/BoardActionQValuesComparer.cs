namespace FifteenPuzzle.Tests.Common;

using System.Diagnostics.CodeAnalysis;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;

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
