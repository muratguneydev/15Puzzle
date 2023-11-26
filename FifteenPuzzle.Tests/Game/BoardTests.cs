using FifteenPuzzle.Game;
using FluentAssertions;
using NUnit.Framework;

namespace FifteenPuzzle.Tests.Game;

public class BoardTests
{
	[Test]
	public void ShouldProvideSolvedBoard()
	{
		Board.Solved.Cells.Should().BeEquivalentTo(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
		});
	}
}