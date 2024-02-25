namespace FifteenPuzzle.Play.Cli.Tests;

using AutoFixture.Kernel;
using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common;

public class BoardDtoSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(BoardDto))
        {        
            return BoardDtoProvider.Get(new RandomBoard());
        }

        return new NoSpecimen();
    }
}