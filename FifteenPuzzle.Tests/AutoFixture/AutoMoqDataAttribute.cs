namespace FifteenPuzzle.Tests.AutoFixture;

using global::AutoFixture.AutoMoq;
using global::AutoFixture.NUnit3;

public class AutoMoqDataAttribute : AutoDataAttribute
{
	public AutoMoqDataAttribute()
		: base(() => AutoFixtureFactory
						.Create()
						.Customize(new AutoMoqCustomization()))
	{
		
	}
}
