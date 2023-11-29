using Spectre.Console;
using FifteenPuzzle.Game;
using FifteenPuzzle.CLI;

// Console.WriteLine("Hello, World!");

// var table = new Table();
// table.AddColumn(new TableColumn("[u]Foo[/]"));
// table.AddColumn(new TableColumn("[u]Bar[/]"));
// table.AddColumn(new TableColumn("[u]Baz[/]"));

// // Add some rows
// table.AddRow("Hello", "[red]World![/]", "");
// table.AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]");
// table.AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

// AnsiConsole.Write(table);

var solver = new DepthFirstSolver(Render);

void Render(Board board)
{
    new BoardRenderer(AnsiConsole.Console, board).Render();
	Thread.Sleep(1000);
}

var board = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 0, 11 },
			{ 13, 14, 15, 12 }
		});
		//Act
		solver.Solve(board);
Console.WriteLine("Complete");
Console.ReadLine();
//new BoardRenderer(AnsiConsole.Console, Board.Solved).Render();

//AnsiConsole.Write(board);
