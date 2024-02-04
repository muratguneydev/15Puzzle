namespace FifteenPuzzle.Tests.Common.AutoFixture;

using FifteenPuzzle.Game;
using global::AutoFixture.Kernel;

public class BoardSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(Board))
        {        
            return new RandomBoard();
        }

        return new NoSpecimen();
    }
}