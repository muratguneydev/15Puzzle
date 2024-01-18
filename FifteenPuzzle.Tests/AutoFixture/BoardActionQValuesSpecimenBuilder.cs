namespace FifteenPuzzle.Tests.AutoFixture;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using global::AutoFixture.Kernel;

public class BoardActionQValuesSpecimenBuilder : ISpecimenBuilder
{
	private static readonly Random Random = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(BoardActionQValues))
        {
			var board = new RandomBoard();
			var nextActionQValues = board.GetMoves().Select(move => new ActionQValue(move, Random.NextDouble()));
			return new BoardActionQValues(board, new ActionQValues(nextActionQValues));
        }

        return new NoSpecimen();
    }
}