using FifteenPuzzle.CLI;
using FifteenPuzzle.Game;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Spectre.Console;

namespace FifteenPuzzle.Tests.CLI;

public class BoardRendererTests
{
	[Test]
	public void ShouldUseCorrectTableForBoard()
    {
        //Arrange
        var consoleSpy = new Mock<IAnsiConsole>();
        var board = Board.Solved;
        var sut = new TableBoardRenderer(consoleSpy.Object, board);
        //Act
        sut.Render();
        //Assert
        consoleSpy.Verify(spy => spy.Write(It.Is<Table>(table => VerifyRenderedTable(table, board))));
    }

    private static Table GetExpectedTable(Board board)
    {
        var expectedTable = new Table();
        expectedTable.AddColumn(new TableColumn("[u]1[/]"));
        expectedTable.AddColumn(new TableColumn("[u]2[/]"));
        expectedTable.AddColumn(new TableColumn("[u]3[/]"));
        expectedTable.AddColumn(new TableColumn("[u]4[/]"));

        foreach (var row in board.Rows)
        {
			expectedTable.AddRow(row.Select(number => number.ToString()).ToArray());

        }

        return expectedTable;
    }

    private static bool VerifyRenderedTable(Table table, Board board)
	{
		var expectedTable = GetExpectedTable(board);

		table.Should().BeEquivalentTo(expectedTable);
		return true;
	}
}