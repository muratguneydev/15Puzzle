namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Text;

public class QValueReader
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;

    public QValueReader(BoardActionQValuesStringConverter boardActionQValuesStringConverter)
    {
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;
    }

    public virtual async Task<IEnumerable<BoardActionQValues>> Read(Stream qValueStream)
    {
        using var reader = new StreamReader(qValueStream, Encoding.UTF8);
        var existingContent = await reader.ReadToEndAsync();
		if (string.IsNullOrEmpty(existingContent))
		{
			return Enumerable.Empty<BoardActionQValues>();
		}

		var csvLines = existingContent.Split(Environment.NewLine);
		var boardActionQValues = csvLines.Select(_boardActionQValuesStringConverter.GetFromLine);

        return boardActionQValues;
    }

    
}
