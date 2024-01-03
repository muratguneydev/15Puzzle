
namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public class QValueWriter : IDisposable
{
    private readonly BoardActionQValuesStringConverter _boardActionQValuesStringConverter;
    private readonly StreamWriter _streamWriter;
	private bool _isDisposed;

    public QValueWriter(BoardActionQValuesStringConverter boardActionQValuesStringConverter, Stream stream)
    {
        _boardActionQValuesStringConverter = boardActionQValuesStringConverter;
        _streamWriter = new StreamWriter(stream);
    }

    public async Task Write(QValueTable qValueTable)
	{
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
}