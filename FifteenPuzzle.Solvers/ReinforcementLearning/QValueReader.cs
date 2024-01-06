namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Text;

public class QValueReader
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;
    private readonly QLearningHyperparameters _parameters;
    private readonly QLearningSystemConfiguration _qLearningSystemConfiguration;
    private readonly FileSystem _fileSystem;

    public QValueReader(BoardActionQValuesStringConverter boardActionQValuesStringConverter,
		QLearningHyperparameters parameters,
		QLearningSystemConfiguration qLearningSystemConfiguration,
		FileSystem fileSystem)
    {
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;
        _parameters = parameters;
        _qLearningSystemConfiguration = qLearningSystemConfiguration;
        _fileSystem = fileSystem;
    }

    public virtual async Task<IEnumerable<BoardActionQValues>> Read()
    {
		if (!_fileSystem.FileExists(_qLearningSystemConfiguration.QValueStorageFilePath))
		{
			return QValueTable.Empty(_parameters);
		}
		
		var qValueStream = _fileSystem.GetFileStreamToRead(_qLearningSystemConfiguration.QValueStorageFilePath);
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
