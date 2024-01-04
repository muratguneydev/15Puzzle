namespace FifteenPuzzle.Tests.Game;

using FifteenPuzzle.Game;
using FluentAssertions;
using NUnit.Framework;
using Shouldly;

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
	public void ShouldIdentifySolvedBoard()
	{
		new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 15, 0 }
		}).IsSolved.Should().BeTrue();
	}

	[Test]
	public void ShouldIdentifyUnsolvedBoard()
	{
		new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 11, 12 },
			{ 13, 14, 0, 15 }
		}).IsSolved.Should().BeFalse();
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
		//Arrange
		var board = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 0, 11, 12 },
			{ 13, 14, 15, 10 }
		});
		//Act
		board.Move("9");
		//Assert
		var expectedBoard = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 0, 9, 11, 12 },
			{ 13, 14, 15, 10 }
		});

		BoardAsserter.ShouldBeEquivalent(expectedBoard, board);
	}

	[Test]
	public void ShouldNotMoveToEmptyCell_WhenNotAdjacent()
	{
		//Arrange
		var board = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 0, 11, 12 },
			{ 13, 14, 15, 10 }
		});
		var expectedBoard = new Board(board);
		//Act
		board.Move("2");
		//Assert
		BoardAsserter.ShouldBeEquivalent(expectedBoard, board);

	}

	[Test]
	public void ShouldProvideFrontierBoards_ForEachPossibleNextMove()
	{
		//Arrange
		var board = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 0, 11, 12 },
			{ 13, 14, 15, 10 }
		});
		
		var expectedFrontierBoards = new []
		{
			new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 0, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			}),
			new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 14, 11, 12 },
				{ 13, 0, 15, 10 }
			}),new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 0, 9, 11, 12 },
				{ 13, 14, 15, 10 }
			}),new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 11, 0, 12 },
				{ 13, 14, 15, 10 }
			}),
		};
		//Act
		var frontierBoards = board.GetFrontierBoards();
		//Assert
		frontierBoards.Should().BeEquivalentTo(expectedFrontierBoards);
	}

	[Test]
	public void ShouldFlatten()
	{
		//Arrange
		var flattened = new Cell[] {
			new (0, 0, "1"), new (0, 1, "2"), new (0, 2, "3"), new (0, 3, "4"),
			new (1, 0, "5"), new (1, 1, "6"), new (1, 2, "7"), new (1, 3, "8"),
			new (2, 0, "9"), new (2, 1, "10"), new (2, 2, "11"), new (2, 3, "12"),
			new (3, 0, "13"), new (3, 1, "14"), new (3, 2, "15"), new (3, 3, "")
		};
		//Act & Assert
		Board.Solved.Flattened.Should().BeEquivalentTo(flattened);
	}

	[Test]
	public void ShouldFindCell()
	{
		//Arrange
		var sut = new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 0, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			});
		//Act & Assert
		sut.GetCell("1").ShouldBe(new Cell(0, 0, "1"));
		sut.GetCell("2").ShouldBe(new Cell(0, 1, "2"));
		sut.GetCell("4").ShouldBe(new Cell(0, 3, "4"));
		sut.GetCell("9").ShouldBe(new Cell(2, 0, "9"));
		sut.GetCell("10").ShouldBe(new Cell(3, 3, "10"));
		sut.GetCell("").ShouldBe(new Cell(1, 1, ""));
	}
}
