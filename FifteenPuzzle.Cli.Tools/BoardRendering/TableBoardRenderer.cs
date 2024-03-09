namespace FifteenPuzzle.Cli.Tools.BoardRendering;

using FifteenPuzzle.Game;
using Spectre.Console;

public class TableBoardRenderer
{
	private readonly IAnsiConsole _console;

	public TableBoardRenderer(IAnsiConsole console) => _console = console;

	public void Render(Board board)
	{
		var table = new Table();
		// table.AddColumn(new TableColumn("[u]1[/]"));
		// table.AddColumn(new TableColumn("[u]2[/]"));
		// table.AddColumn(new TableColumn("[u]3[/]"));
		// table.AddColumn(new TableColumn("[u]4[/]"));
		table.AddColumn(new TableColumn("+"));
		table.AddColumn(new TableColumn("+"));
		table.AddColumn(new TableColumn("+"));
		table.AddColumn(new TableColumn("+"));

		foreach(var row in board.Rows)
		{
			table.AddRow(row.Select(cell => new TableCellRenderer(cell, board).Render()).ToArray());
		}

		_console.Write(table);
	}
}
