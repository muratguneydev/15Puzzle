namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class QValueWriter
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;
    private readonly QLearningSystemConfiguration _qLearningSystemConfiguration;
    private readonly FileSystem _fileSystem;

    public QValueWriter(BoardActionQValuesStringConverter boardActionQValuesStringConverter,
		QLearningSystemConfiguration qLearningSystemConfiguration, FileSystem fileSystem)
    {
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;
        _qLearningSystemConfiguration = qLearningSystemConfiguration;
        _fileSystem = fileSystem;
    }

    public virtual async Task<IDisposable> Write(QValueTable qValueTable)
    {
		var stream = _fileSystem.GetFileStreamToWrite(_qLearningSystemConfiguration.QValueStorageFilePath);
        var streamWriter = new StreamWriter(stream);//don't dispose as it will dispose the stream too.

        var lines = _boardActionQValuesStringConverter.GetBoardQValueFileContent(qValueTable);

        await streamWriter.WriteAsync(lines);
        await streamWriter.FlushAsync();
		return streamWriter;
    }
}
