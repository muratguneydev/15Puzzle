namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class QValueWriter : IDisposable
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;
    private readonly QLearningSystemConfiguration _qLearningSystemConfiguration;
    private readonly FileSystem _fileSystem;
    private bool _isDisposed;
    private StreamWriter _streamWriter = GetDefaultStreamWriter();

    public QValueWriter(BoardActionQValuesStringConverter boardActionQValuesStringConverter,
		QLearningSystemConfiguration qLearningSystemConfiguration, FileSystem fileSystem)
    {
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;
        _qLearningSystemConfiguration = qLearningSystemConfiguration;
        _fileSystem = fileSystem;
    }

    public virtual async Task Write(QValueTable qValueTable)
    {
		var stream = _fileSystem.GetFileStreamToWrite(_qLearningSystemConfiguration.QValueStorageFilePath);
        _streamWriter = new StreamWriter(stream);//don't dispose as it will dispose the stream too.

        var lines = string.Join(Environment.NewLine, qValueTable.Select(_boardActionQValuesStringConverter.GetLine));

        await _streamWriter.WriteAsync(lines);
        await _streamWriter.FlushAsync();
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _streamWriter.Dispose();
        GC.SuppressFinalize(this);//still needed to avoid finalizer queue.
        _isDisposed = true;
    }

    private static StreamWriter GetDefaultStreamWriter() => new (new MemoryStream());

}
