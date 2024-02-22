namespace FifteenPuzzle.Cli.Tools.BoardRendering;

using Spectre.Console;
using FifteenPuzzle.Game;

public class BoardText
{
    private readonly Board _board;

    public BoardText(Board board) => _board = board;

    public string Text => $"{Environment.NewLine}{string.Join(Environment.NewLine, _board.Rows.Select(row => new RowText(row).Text))}";

	private class CellText
	{
        private readonly Cell _cell;

        public CellText(Cell cell) => _cell = cell;

		public string Text => $" {_cell.Value,-2} ";
    }

	private class RowText
	{
        private readonly Row _row;

        public RowText(Row row) => _row = row;

		public string Text => string.Join('|', _row.Select(cell => new CellText(cell).Text));
    }
}