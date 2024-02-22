namespace FifteenPuzzle.Cli.Tools.BoardRendering;

using FifteenPuzzle.Game;
using Spectre.Console;

public class FlatBoardRenderer
{
	private readonly IAnsiConsole _console;
	private readonly Board _board;

	public FlatBoardRenderer(IAnsiConsole console, Board board)
	{
		_console = console;
		_board = board;
	}

	public void Render()
	{
		foreach(var cell in _board.Flattened)
		{
			_console.Markup(new FlatCellRenderer(cell, _board).Render());
		}
		_console.WriteLine();
	}
}
