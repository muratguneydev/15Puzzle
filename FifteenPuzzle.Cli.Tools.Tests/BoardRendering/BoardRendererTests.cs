namespace FifteenPuzzle.Cli.Tools.Rendering.Tests;

using FifteenPuzzle.Game;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Spectre.Console;
using FifteenPuzzle.Cli.Tools.BoardRendering;

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
        AddColumns(expectedTable);
        AddRows(board, expectedTable);
        return expectedTable;
    }

    private static void AddRows(Board board, Table expectedTable)
    {
        foreach (var row in board.Rows)
        {
            expectedTable.AddRow(row.Select(number => number.ToString()).ToArray());
        }
    }

    private static void AddColumns(Table expectedTable)
    {
        expectedTable.AddColumn(new TableColumn("[u]1[/]"));
        expectedTable.AddColumn(new TableColumn("[u]2[/]"));
        expectedTable.AddColumn(new TableColumn("[u]3[/]"));
        expectedTable.AddColumn(new TableColumn("[u]4[/]"));
    }

    private static bool VerifyRenderedTable(Table table, Board board)
	{
		var expectedTable = GetExpectedTable(board);

		table.Should().BeEquivalentTo(expectedTable);
		return true;
	}
}