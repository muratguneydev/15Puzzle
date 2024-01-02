namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

using System.Text;
using FifteenPuzzle.Game;

public class QValueReader
{
    private readonly Stream _qValueStream;

    public QValueReader(Stream qValueStream) => _qValueStream = qValueStream;

	public QValueTable Read()
    {
        using var reader = new StreamReader(_qValueStream, Encoding.UTF8);
        var fileContent = reader.ReadToEnd();
		if (string.IsNullOrEmpty(fileContent))
		{
			return new EmptyQValueTable();
		}

		var csvLines = fileContent.Split(Environment.NewLine);
		var boardActionQValues = csvLines.Select(GetBoardActionQValuesFromLine);

        return new QValueTable(boardActionQValues);
    }

    private static BoardActionQValues GetBoardActionQValuesFromLine(string csv)
    {
        var values = csv.Split(',');

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
}
