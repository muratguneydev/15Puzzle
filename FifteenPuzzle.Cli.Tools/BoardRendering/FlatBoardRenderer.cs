namespace FifteenPuzzle.Cli.Tools.BoardRendering;

using FifteenPuzzle.Game;
using Spectre.Console;

public class FlatBoardRenderer
{
	private readonly IAnsiConsole _console;

    public FlatBoardRenderer(IAnsiConsole console) => _console = console;

    public void Render(Board board)
	{
		foreach(var cell in board.Flattened)
		{
			_console.Markup(new FlatCellRenderer(cell, board).Render());
		}
		_console.WriteLine();
	}
}
