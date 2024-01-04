namespace FifteenPuzzle.Tests.AutoFixture;

using global::AutoFixture;

public class AutoFixtureFactory
{
	public static IFixture Create() => new Fixture();
}
