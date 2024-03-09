namespace FifteenPuzzle.Play.Cli;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using Spectre.Console;

public class SuggestionsRenderer
{
	private readonly IAnsiConsole _console;

    public SuggestionsRenderer(IAnsiConsole console) => _console = console;

    public void Render(ActionQValues actionQValues)
	{
		_console.Write("Suggestions:");
		if (!actionQValues.Any())
		{
			_console.WriteLine("None found...");
			return;
		}
		
		foreach(var actionQValue in actionQValues)
		{
			_console.Markup($"[blue]*{actionQValue.Move.Number}:{actionQValue.QValue}*[/]");
		}
		_console.WriteLine();
	}
}
