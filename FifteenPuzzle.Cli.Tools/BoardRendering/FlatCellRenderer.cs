namespace FifteenPuzzle.Cli.Tools.BoardRendering;

using FifteenPuzzle.Game;

public class FlatCellRenderer
{
    private readonly Cell _cell;
	private readonly bool _isMovable;

    public FlatCellRenderer(Cell cell, Board board)
	{
        _cell = cell;
		_isMovable = board.GetMovableCells().Contains(cell);
    }

    public string Render() => (_isMovable ? $"[blue]*{_cell.Value}*[/]" : _cell.Value) + ",";
}