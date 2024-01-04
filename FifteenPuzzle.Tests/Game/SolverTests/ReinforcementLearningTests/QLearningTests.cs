namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FluentAssertions;
using global::AutoFixture.NUnit3;
using NUnit.Framework;

public class QLearningTests
{
	[Test, AutoData]
	public void ShouldStopAfterNIterations(QLearningHyperparameters learningParameters, int numberOfIterations)
	{
		//Arrange
		learningParameters = learningParameters with { NumberOfIterations = numberOfIterations };
		var qLearning = new QLearning(learningParameters);
		//Act
		qLearning.Learn();
		//Assert
		qLearning.NumberOfIterations.Should().Be(numberOfIterations);
	}

	[Test, AutoData]
	public void ShouldInitializeQValuesBeforeStart(QLearningHyperparameters learningParameters)
	{
		//Arrange
		learningParameters = learningParameters with { NumberOfIterations = 0 };
		var qLearning = new QLearning(learningParameters);
		//Act
		qLearning.Learn();
		//Assert
		//qLearning.NumberOfIterations.Should().Be(numberOfIterations);
	}
}
