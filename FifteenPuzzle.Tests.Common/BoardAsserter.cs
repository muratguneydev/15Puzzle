namespace FifteenPuzzle.Tests.Common;

using FifteenPuzzle.Game;
using Shouldly;

public static class BoardAsserter
{
    public static void ShouldBeEquivalent(Board expected, Board actual) => actual.ShouldBe(expected, new BoardComparer());

    public static void ShouldBeEquivalent(IEnumerable<Board> expected, IEnumerable<Board> actual) =>
		actual.ShouldBe(expected, new BoardComparer(), ignoreOrder: true);
}
