using AutoFixture.NUnit3;
using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using Moq;
using NUnit.Framework;

namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

public class EpsilonGreedyActionSelectionPolicyTests
{
	[Test, AutoMoqData]
	public void ShouldExplore_WhenRandomIsLessThan_ExplorationProbabilityEpsilon(
		[Frozen] [Mock] Mock<Random> randomStub,
		EpsilonGreedyActionSelectionPolicy sut
	)
	{
		//Arrange
		//var explorationProbability = 0.9;
		var randomLessThanExplorationProbability = 0.3;
        randomStub.Setup(r => r.NextDouble()).Returns(randomLessThanExplorationProbability);
		//Act
		//var action = sut.GetAction();
        

	}
}