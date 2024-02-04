namespace FifteenPuzzle.Tests.AutoFixture;

using global::AutoFixture.Kernel;

public class CancellationTokenSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(CancellationToken))
        {        
            return new CancellationToken(false);
        }

        return new NoSpecimen();
    }
}
