namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

using System.Collections;
using FifteenPuzzle.Game;

public record QValueTable : IEnumerable<BoardActionQValues>
{
	private readonly Dictionary<int,BoardActionQValues> _boardActionQValues;
	private static readonly BoardComparer _boardComparer = new();

    public QValueTable(IEnumerable<BoardActionQValues> boardActionQValues) =>
		_boardActionQValues = boardActionQValues.ToDictionary(b => _boardComparer.GetHashCode(b.Board));

    public ActionQValues Get(Board board)
	{
		//TODO: throw or return empty when not found
		return _boardActionQValues[_boardComparer.GetHashCode(board)].ActionQValues;
	}

	//public 

    public IEnumerator<BoardActionQValues> GetEnumerator() => _boardActionQValues.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
