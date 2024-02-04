namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests.ActionSelectionTests;

using AutoFixture.NUnit3;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;

public class EpsilonGreedyActionSelectionPolicyFactoryTests
{
	[Test, AutoMoqData]
	public void Should_ReturnExplore_WhenRandomIsLessThan_ExplorationProbabilityEpsilon(
		[Frozen] [Mock] Mock<Random> randomStub,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		EpsilonGreedyActionSelectionPolicyFactory sut)
	{
		//Arrange
		var explorationProbabilityEpsilon = 0.9;
		var randomLessThanExplorationProbability = 0.3;
        randomStub
			.Setup(r => r.NextDouble())
			.Returns(randomLessThanExplorationProbability);
		learningParametersStub
			.SetupGet(stub => stub.ExplorationProbabilityEpsilon)
			.Returns(explorationProbabilityEpsilon);
		//Act
		var result = sut.Get();
        //Assert
		result.ShouldBeOfType<EpsilonGreedyExploreActionSelectionPolicy>();
	}

	[Test, AutoMoqData]
	public void ShouldExploit_WhenRandomIsGreaterThan_ExplorationProbabilityEpsilon(
		[Frozen] [Mock] Mock<Random> randomStub,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		EpsilonGreedyActionSelectionPolicyFactory sut)
	{
		//Arrange
		var explorationProbabilityEpsilon = 0.6;
		var randomGreaterThanExplorationProbability = 0.8;
        randomStub
			.Setup(r => r.NextDouble())
			.Returns(randomGreaterThanExplorationProbability);
		learningParametersStub
			.SetupGet(stub => stub.ExplorationProbabilityEpsilon)
			.Returns(explorationProbabilityEpsilon);
		//Act
		var result = sut.Get();
        //Assert
		result.ShouldBeOfType<EpsilonGreedyExploitActionSelectionPolicy>();
	}

	[Test, AutoMoqData]
	public void ShouldExploit_WhenRandomEquals_ExplorationProbabilityEpsilon(
		[Frozen] [Mock] Mock<Random> randomStub,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		EpsilonGreedyActionSelectionPolicyFactory sut)
	{
		//Arrange
		var explorationProbabilityEpsilon = 0.6;
		var randomSameAsExplorationProbability = explorationProbabilityEpsilon;
        randomStub
			.Setup(r => r.NextDouble())
			.Returns(randomSameAsExplorationProbability);
		learningParametersStub
			.SetupGet(stub => stub.ExplorationProbabilityEpsilon)
			.Returns(explorationProbabilityEpsilon);
		//Act
		var result = sut.Get();
        //Assert
		result.ShouldBeOfType<EpsilonGreedyExploitActionSelectionPolicy>();
	}
}
