namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using Shouldly;

public class ActionQValuesTests
{
    [Test, DomainAutoData]
    public void ShouldFindMaxQValue_WhenNotEmpty(ActionQValues sut) => sut.MaxQValue.ShouldBe(sut.MaxBy(a => a.QValue)!.QValue);

	[Test, DomainAutoData]
    public void ShouldProvide0MaxQValue_WhenEmpty() => new ActionQValues(Enumerable.Empty<ActionQValue>()).MaxQValue.ShouldBe(0);

	[Test, DomainAutoData]
	public void ShouldRemoveWithASeparateCollection(ActionQValues sut)
	{
		//Arrange
		var snapshot = sut.ToList();
		//Act
		var result = sut.Remove(sut.First());
		//Assert
		sut.Should().BeEquivalentTo(snapshot);
		result.Should().BeEquivalentTo(snapshot.Skip(1));
	}

	[Test, DomainAutoData]
	public void ShouldFindByMove(ActionQValues sut)
	{
		//Arrange
		var actionToMatch = sut.First();
		var actionToFind = new ActionQValue(new Move(actionToMatch.Move.Number), default);
		//Act
		var result = sut.Get(actionToFind);
		//Assert
		result.ShouldBe(actionToMatch);
	}
}