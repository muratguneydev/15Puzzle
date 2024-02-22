namespace FifteenPuzzle.Solver.Cli;

using FifteenPuzzle.Game;
using Spectre.Console;

public class TableCellRenderer
{
    private readonly Cell _cell;
	private readonly bool _isMovable;

    public TableCellRenderer(Cell cell, Board board)
	{
        _cell = cell;
		_isMovable = board.GetMovableCells().Contains(cell);
    }

    //public IRenderable Render() => new Markup($"[blue]{_cell.Value}[/]");
    public string Render() => _isMovable ? $"[blue]{_cell.Value}[/]" : _cell.Value;
}
