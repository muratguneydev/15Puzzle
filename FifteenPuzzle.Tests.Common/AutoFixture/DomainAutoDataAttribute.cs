namespace FifteenPuzzle.Tests.Common.AutoFixture;

using global::AutoFixture.NUnit3;

public class DomainAutoDataAttribute : AutoDataAttribute
{
	public DomainAutoDataAttribute()
		: base(AutoFixtureFactory.Create)
	{
		
	}
}
