namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests.ActionSelectionTests;

using AutoFixture.NUnit3;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;

public class EpsilonGreedyExploreActionSelectionPolicyTests
{
	[Test, AutoMoqData]
	public void ShouldPickRandomAction(
		ActionQValues actionQValueCollection,
		[Frozen] [Mock] Mock<Random> randomStub,
		EpsilonGreedyExploreActionSelectionPolicy sut
	)
	{
		//Arrange
		var actionQValues = actionQValueCollection.ToArray();
		const int indexToPick = 1;
        randomStub.Setup(r => r.Next(0, actionQValues.Length-1)).Returns(indexToPick);

		var expectedRandomExploreActionQValue = actionQValues[indexToPick];
		//Act
		var result = sut.PickAction(actionQValueCollection);
        //Assert
		result.ShouldBe(expectedRandomExploreActionQValue);
	}
}
