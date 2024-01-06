namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using NUnit.Framework;
using Shouldly;

public class ActionQValuesTests
{
    [Test, DomainAutoData]
    public void ShouldFindMaxQValue_WhenNotEmpty(ActionQValues sut) => sut.MaxQValue.ShouldBe(sut.MaxBy(a => a.QValue)!.QValue);

	[Test, DomainAutoData]
    public void ShouldProvide0MaxQValue_WhenEmpty() => ActionQValues.Empty.MaxQValue.ShouldBe(0);
}