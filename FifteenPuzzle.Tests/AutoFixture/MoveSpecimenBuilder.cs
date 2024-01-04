namespace FifteenPuzzle.Tests.AutoFixture;

using FifteenPuzzle.Game;
using global::AutoFixture.Kernel;

public class MoveSpecimenBuilder : ISpecimenBuilder
{
	private static readonly Random Random = new();
	private static readonly int MaxNumber = Board.SideLength * Board.SideLength - 1;

    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Move))
        {        
            return new Move(Random.Next(1, MaxNumber));
        }

        return new NoSpecimen();
    }
}