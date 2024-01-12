namespace FifteenPuzzle.CLI;

using Spectre.Console;
using FifteenPuzzle.Game;

public class Renderer
{
    private readonly IAnsiConsole _console;

    public Renderer(IAnsiConsole console) => _console = console;

    public void Render(Board board)
	{
		new TableBoardRenderer(_console, board).Render();
		new FlatBoardRenderer(_console, board).Render();
		Thread.Sleep(1000);
	}
}
