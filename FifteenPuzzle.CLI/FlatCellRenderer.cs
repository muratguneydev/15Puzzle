namespace FifteenPuzzle.CLI;

using FifteenPuzzle.Game;
using Spectre.Console;

public class FlatCellRenderer
{
    private readonly Cell _cell;
	private readonly bool _isMovable;

    public FlatCellRenderer(Cell cell, Board board)
	{
        _cell = cell;
		_isMovable = board.GetMovableCells().Select(x => x.Value).Contains(cell.Value);
    }

    //public IRenderable Render() => new Markup($"[blue]{_cell.Value}[/]");
    public string Render() => (_isMovable ? $"[blue]*{_cell.Value}*[/]" : _cell.Value) + ",";
}