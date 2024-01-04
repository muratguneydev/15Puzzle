namespace FifteenPuzzle.Tests.AutoFixture;

using System.Reflection;
using global::AutoFixture;

public class MockAttribute : Attribute, IParameterCustomizationSource
{
    public ICustomization GetCustomization(ParameterInfo parameter) => new MockCustomization(parameter);
}
