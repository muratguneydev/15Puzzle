namespace FifteenPuzzle.CLI.Commands;

using FifteenPuzzle.CLI;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers;
using System.CommandLine;

public class DepthFirstCommand : Command
{
	//dotnet run depthfirst --useEasyBoard

    //https://github.com/KeesCBakker/dotnet-cli-di-poc/blob/main/src/MyCli/Commands/CurrentCommand.cs
	//https://spectreconsole.net/cli/commands
	//https://nikiforovall.github.io/dotnet/2022/08/26/persisted-parameters-in-dotnet-cli.html

    private readonly DepthFirstSolver _solver;
    private readonly Renderer _renderer;
    private static readonly Board EasyBoard = new (new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 0, 11 },
			{ 13, 14, 15, 12 }
		});

    public DepthFirstCommand(DepthFirstSolver solver, Renderer renderer)
		: base("depthfirst", "Solves the 15 puzzle using depth first search.")
    {
        _solver = solver;
        _renderer = renderer;

		_solver.AddOnNewItemTested(_renderer.Render);

        var useEasyBoardOption = new Option<bool>(new[] { "--useEasyBoard", "-e" }, () => false, "Easy or random puzzle.");
        AddOption(useEasyBoardOption);
        this.SetHandler(Execute, useEasyBoardOption);
    }

    private void Execute(bool useEasyBoard)
    {
        var board = useEasyBoard ? EasyBoard : new RandomBoard();
        _solver.Solve(board);
		Console.WriteLine("Complete.");
    }
}
