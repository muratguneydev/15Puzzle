namespace FifteenPuzzle.Tests.AutoFixture;

using global::AutoFixture;

public class AutoFixtureFactory
{
    public static IFixture Create()
    {
        var fixture = new Fixture();
		fixture.Customizations.Add(new MoveSpecimenBuilder());
		return fixture;
    }
}
