using Spectre.Console;
using FifteenPuzzle.Game;

Console.WriteLine("Hello, World!");

var table = new Table();
table.AddColumn(new TableColumn("[u]Foo[/]"));
table.AddColumn(new TableColumn("[u]Bar[/]"));
table.AddColumn(new TableColumn("[u]Baz[/]"));

// Add some rows
table.AddRow("Hello", "[red]World![/]", "");
table.AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]");
table.AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

AnsiConsole.Write(table);

var board = new Board(new[,] { 
	{ 1, 2, 3, 4 },
	{ 5, 6, 7, 8 },
	{ 9, 10, 11, 12 },
	{ 13, 14, 15, 0 }
	});

//AnsiConsole.Write(board);
