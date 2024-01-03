namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardActionQValuesStringConverter
{
	private const char Separator = ',';
	public BoardActionQValues GetFromLine(string csv)
    {
        var values = csv.Split(Separator);

        var cells = new string[Board.SideLength, Board.SideLength];
        var boardValueItemLength = Board.SideLength * Board.SideLength;
        for (int index = 0; index < boardValueItemLength; index++)
        {
            var rowIndex = index / Board.SideLength;
            var colIndex = index % Board.SideLength;
            cells[rowIndex, colIndex] = values[index];
        }

        var up = double.Parse(values[boardValueItemLength]);
        var right = double.Parse(values[boardValueItemLength + 1]);
        var down = double.Parse(values[boardValueItemLength + 2]);
        var left = double.Parse(values[boardValueItemLength + 3]);
        var actionQValues = new ActionQValues(up, right, down, left);
        var board = new Board(cells);
        var boardActionQValues = new BoardActionQValues(board, actionQValues);
        return boardActionQValues;
    }

    public string GetLine(BoardActionQValues boardActionQValues) =>
		$"{GetBoardString(boardActionQValues.Board)}{Separator}{GetActionQValuesString(boardActionQValues.ActionQValues)}";

    private object GetActionQValuesString(ActionQValues actionQValues) =>
		$"{actionQValues.Up}{Separator}{actionQValues.Right}{Separator}{actionQValues.Down}{Separator}{actionQValues.Left}";

    private object GetBoardString(Board board) =>
		string.Join(Separator, board.Flattened.Select(cell => cell.Value));
}