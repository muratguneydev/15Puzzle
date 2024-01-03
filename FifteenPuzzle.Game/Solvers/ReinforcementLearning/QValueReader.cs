namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

using System.Text;

public class QValueReader
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;
    private readonly Stream _qValueStream;

    public QValueReader(BoardActionQValuesStringConverter boardActionQValuesStringConverter, Stream qValueStream)
    {
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;
        _qValueStream = qValueStream;
    }

    public async Task<QValueTable> Read()
    {
        using var reader = new StreamReader(_qValueStream, Encoding.UTF8);
        var existingContent = await reader.ReadToEndAsync();
		if (string.IsNullOrEmpty(existingContent))
		{
			return new EmptyQValueTable();
		}

		var csvLines = existingContent.Split(Environment.NewLine);
		var boardActionQValues = csvLines.Select(_boardActionQValuesStringConverter.GetFromLine);

        return new QValueTable(boardActionQValues);
    }

    
}
