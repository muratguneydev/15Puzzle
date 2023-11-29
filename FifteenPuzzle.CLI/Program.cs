using Spectre.Console;
using FifteenPuzzle.Game;
using FifteenPuzzle.CLI;

var solver = new DepthFirstSolver(Render);

var board = new Board(new[,]
	{
		{ 1, 2, 3, 4 },
		{ 5, 6, 7, 8 },
		{ 9, 10, 0, 11 },
		{ 13, 14, 15, 12 }
	});
		
solver.Solve(board);
Console.WriteLine("Complete.");
//Console.ReadLine();

void Render(Board board)
{
    new TableBoardRenderer(AnsiConsole.Console, board).Render();
	new FlatBoardRenderer(AnsiConsole.Console, board).Render();
	Thread.Sleep(1000);
}
