using Spectre.Console;
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