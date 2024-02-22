namespace FifteenPuzzle.Tests;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FifteenPuzzle.Tests.Common.AutoFixture;

public class ApiAutoDataAttribute : AutoDataAttribute
{
	public ApiAutoDataAttribute()
		: base(Create)
	{
		
	}

	public static IFixture Create()
    {
        var fixture = AutoFixtureFactory.Create();
		fixture.Customizations.Add(new BoardDtoSpecimenBuilder());
		return fixture;
    }
}

public class ApiAutoMoqDataAttribute : AutoDataAttribute
{
	public ApiAutoMoqDataAttribute()
		: base(Create)
	{
		
	}

	public static IFixture Create()
    {
        var fixture = AutoFixtureFactory
						.Create()
						.Customize(new AutoMoqCustomization());
		fixture.Customizations.Add(new BoardDtoSpecimenBuilder());
		return fixture;
    }
}