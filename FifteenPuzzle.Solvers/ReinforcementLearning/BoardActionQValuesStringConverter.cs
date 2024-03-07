namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardActionQValuesStringConverter
{
	private const char Separator = ',';
	private const char ActionQValueSeparator = '/';

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

		var actionQValues = values
			.TakeLast(values.Length - boardValueItemLength)
			.Select(GetActionQValue);

		var board = new Board(cells);
        var boardActionQValues = new BoardActionQValues(board, new ActionQValues(actionQValues));
        return boardActionQValues;
    }

    public string GetLine(BoardActionQValues boardActionQValues) =>
		$"{GetBoardString(boardActionQValues.Board)}{Separator}{GetActionQValuesString(boardActionQValues.ActionQValues)}";

    public string GetBoardQValueFileContent(IEnumerable<BoardActionQValues> boardActionQValuesCollection) =>
		string.Join(Environment.NewLine, boardActionQValuesCollection.Select(GetLine));

    private ActionQValue GetActionQValue(string actionQValueString)
    {
        var parts = actionQValueString.Split(ActionQValueSeparator);
		return new ActionQValue(new Move(int.Parse(parts[0])), double.Parse(parts[1]));
    }

    private string GetActionQValuesString(ActionQValues actionQValues) =>
		string.Join(Separator, actionQValues.Select(a => $"{a.Move.Number}{ActionQValueSeparator}{a.QValue}"));

    private string GetBoardString(Board board) =>
		string.Join(Separator, board.Flattened.Select(cell => cell.Value));
}