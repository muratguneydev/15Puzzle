namespace FifteenPuzzle.Tests.Common.AutoFixture;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using global::AutoFixture.Kernel;

public class ActionQValuesSpecimenBuilder : ISpecimenBuilder
{
	private const int max = 3;
	private static readonly Random Random = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(ActionQValues))
        {        
            return new ActionQValues(new[] {
				new ActionQValue(new Move(Random.Next(1, max)), Random.NextDouble()),
				new ActionQValue(new Move(Random.Next(max+1, max+4)), Random.NextDouble()),
				new ActionQValue(new Move(Random.Next(max+5, max+8)), Random.NextDouble())
			});
        }

        return new NoSpecimen();
    }
}
