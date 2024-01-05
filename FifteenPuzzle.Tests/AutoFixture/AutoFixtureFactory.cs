namespace FifteenPuzzle.Tests.AutoFixture;

using global::AutoFixture;

public class AutoFixtureFactory
{
    public static IFixture Create()
    {
        var fixture = new Fixture();
		fixture.Customizations.Add(new MoveSpecimenBuilder());
		fixture.Customizations.Add(new BoardSpecimenBuilder());
		fixture.Customizations.Add(new BoardActionQValuesSpecimenBuilder());
		return fixture;
    }
}
