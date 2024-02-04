namespace FifteenPuzzle.Tests.AutoFixture;

using global::AutoFixture;

public class AutoFixtureFactory
{
    public static IFixture Create()
    {
        var fixture = new Fixture();
		fixture.Customizations.Add(new ActionQValuesSpecimenBuilder());
		fixture.Customizations.Add(new BoardActionQValuesSpecimenBuilder());
		fixture.Customizations.Add(new BoardSpecimenBuilder());
		fixture.Customizations.Add(new CancellationTokenSpecimenBuilder());
		fixture.Customizations.Add(new MoveSpecimenBuilder());
		return fixture;
    }
}
