namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class QValueWriter : IDisposable
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;
    private bool _isDisposed;
    private StreamWriter _streamWriter = GetDefaultStreamWriter();

    public QValueWriter(BoardActionQValuesStringConverter boardActionQValuesStringConverter) =>
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;

    public async Task Write(QValueTable qValueTable, Stream stream)
    {
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

    private static StreamWriter GetDefaultStreamWriter() => new StreamWriter(new MemoryStream());

}