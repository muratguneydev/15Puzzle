namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Shouldly;

public class EpsilonGreedyActionSelectionPolicyTests
{
	[Test, AutoMoqData]
	public void ShouldExplore_WhenRandomIsLessThan_ExplorationProbabilityEpsilon(
		BoardActionQValues boardActionQValues,
		[Frozen] [Mock] Mock<Random> randomStub,
		EpsilonGreedyActionSelectionPolicy sut
	)
	{
		//Arrange
		var explorationProbabilityEpsilon = 0.9;
		var randomLessThanExplorationProbability = 0.3;
        randomStub.Setup(r => r.NextDouble()).Returns(randomLessThanExplorationProbability);

		var actionQValues = boardActionQValues.ActionQValues.ToArray();
		const int indexToPick = 1;
        randomStub.Setup(r => r.Next(0, actionQValues.Length-1)).Returns(indexToPick);

		var expectedRandomExploreActionQValue = actionQValues[indexToPick];
		//Act
		var result = sut.PickAction(boardActionQValues, explorationProbabilityEpsilon);
        //Assert
		result.ShouldBe(expectedRandomExploreActionQValue);
	}

	[Test, AutoMoqData]
	public void ShouldExploit_WhenRandomIsGreaterThan_ExplorationProbabilityEpsilon(
		BoardActionQValues boardActionQValues,
		[Frozen] [Mock] Mock<Random> randomStub,
		EpsilonGreedyActionSelectionPolicy sut
	)
	{
		//Arrange
		var explorationProbabilityEpsilon = 0.6;
		var randomGreaterThanExplorationProbability = 0.8;
        randomStub.Setup(r => r.NextDouble()).Returns(randomGreaterThanExplorationProbability);

		var actionQValues = boardActionQValues.ActionQValues.ToArray();
		var expectedExploitActionWithMaxQValue = actionQValues.MaxBy(a => a.QValue);
		//Act
		var result = sut.PickAction(boardActionQValues, explorationProbabilityEpsilon);
        //Assert
		result.ShouldBe(expectedExploitActionWithMaxQValue);
	}

	[Test, AutoMoqData]
	public void ShouldExploit_WhenRandomEquals_ExplorationProbabilityEpsilon(
		BoardActionQValues boardActionQValues,
		[Frozen] [Mock] Mock<Random> randomStub,
		EpsilonGreedyActionSelectionPolicy sut
	)
	{
		//Arrange
		var explorationProbabilityEpsilon = 0.6;
		var randomSameAsExplorationProbability = explorationProbabilityEpsilon;
        randomStub.Setup(r => r.NextDouble()).Returns(randomSameAsExplorationProbability);

		var actionQValues = boardActionQValues.ActionQValues.ToArray();
		var expectedExploitActionWithMaxQValue = actionQValues.MaxBy(a => a.QValue);
		//Act
		var result = sut.PickAction(boardActionQValues, explorationProbabilityEpsilon);
        //Assert
		result.ShouldBe(expectedExploitActionWithMaxQValue);
	}
}