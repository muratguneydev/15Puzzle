namespace FifteenPuzzle.Tests;

using FifteenPuzzle.Game;
using FluentAssertions;
using Shouldly;

public static class BoardAsserter
{
    public static void ShouldBeEquivalent(Board expected, Board actual) => actual.ShouldBe(expected, new BoardComparer());

    public static void ShouldBeEquivalent(IEnumerable<Board> expected, IEnumerable<Board> actual)
	{
		var expectedBoards = expected.ToArray();
		var actualBoards = actual.ToArray();

		actualBoards.Should().HaveSameCount(expectedBoards);

		for (var i=0;i < actualBoards.Length;i++)
		{
			ShouldBeEquivalent(expectedBoards[i], actualBoards[i]);
		}
	}
}