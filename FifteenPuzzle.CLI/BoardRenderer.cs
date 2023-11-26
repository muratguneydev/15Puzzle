namespace FifteenPuzzle.CLI;

using FifteenPuzzle.Game;
using Spectre.Console;
using Spectre.Console.Advanced;

public class BoardRenderer
{
	private readonly IAnsiConsole _console;
	private readonly Board _board;

	public BoardRenderer(IAnsiConsole console, Board board)
	{
		_console = console;
		_board = board;
	}

	public void Render()
	{
		var table = new Table();
		table.AddColumn(new TableColumn("[u]1[/]"));
		table.AddColumn(new TableColumn("[u]2[/]"));
		table.AddColumn(new TableColumn("[u]3[/]"));
		table.AddColumn(new TableColumn("[u]4[/]"));

		foreach(var row in _board.Rows)
		{
			//table.AddRow(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString());
			table.AddRow(row.Select(number => number.ToString()).ToArray());
		}

		_console.Write(table);
	}
}