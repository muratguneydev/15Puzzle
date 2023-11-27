using FifteenPuzzle.Game;
using FluentAssertions;
using NUnit.Framework;

namespace FifteenPuzzle.Tests.Game;

public class BoardTests
{
	[Test]
	public void ShouldProvideSolvedBoard()
	{
		Board.Solved.Cells.Should().BeEquivalentTo(new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
		}).Cells);
	}

    [Test]
    public void ShouldProvideRows() => Board.Solved.Should().BeEquivalentTo(new Board(new[,]
        {
            { 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
        }));

    [Test]
	public void ShouldMoveToEmptyCell_WhenAdjacent()
	{
		var board1 = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 0, 11, 12 },
			{ 13, 14, 15, 10 }
		});

		board1.Move(9);

		var board2 = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 0, 9, 11, 12 },
			{ 13, 14, 15, 10 }
		});

		board1.Cells.Should().BeEquivalentTo(board2.Cells);
		board1.Rows.Should().BeEquivalentTo(board2.Rows);
	}

	[Test]
	public void ShouldNotMoveToEmptyCell_WhenNotAdjacent()
	{
		var board1 = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 0, 11, 12 },
			{ 13, 14, 15, 10 }
		});
		var board2 = new Board(board1);

		board1.Move(2);

		board1.Cells.Should().BeEquivalentTo(board2.Cells);
		board1.Rows.Should().BeEquivalentTo(board2.Rows);
	}
}