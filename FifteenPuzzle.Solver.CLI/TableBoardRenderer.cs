namespace FifteenPuzzle.Solver.Cli;

using FifteenPuzzle.Game;
using Spectre.Console;

public class TableBoardRenderer
{
	private readonly IAnsiConsole _console;
	private readonly Board _board;

	public TableBoardRenderer(IAnsiConsole console, Board board)
	{
		_console = console;
		_board = board;
	}

	public void Render()
	{
		var table = new Table();
		// table.AddColumn(new TableColumn("[u]1[/]"));
		// table.AddColumn(new TableColumn("[u]2[/]"));
		// table.AddColumn(new TableColumn("[u]3[/]"));
		// table.AddColumn(new TableColumn("[u]4[/]"));
		table.AddColumn(new TableColumn("1"));
		table.AddColumn(new TableColumn("2"));
		table.AddColumn(new TableColumn("3"));
		table.AddColumn(new TableColumn("4"));

		foreach(var row in _board.Rows)
		{
			table.AddRow(row.Select(cell => new TableCellRenderer(cell, _board).Render()).ToArray());
		}

		_console.Write(table);
	}
}
