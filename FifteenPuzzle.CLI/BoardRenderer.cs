namespace FifteenPuzzle.CLI;

using FifteenPuzzle.Game;
using Spectre.Console;

public class BoardRenderer
{
	private readonly IAnsiConsole _console;
	private readonly Board _board;

	public BoardRenderer(IAnsiConsole console, Board board)
	{
		_console = console;
		_board = board;
	}
}