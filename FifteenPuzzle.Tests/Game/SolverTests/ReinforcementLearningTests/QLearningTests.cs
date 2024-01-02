namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

using AutoFixture.NUnit3;
using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FluentAssertions;
using NUnit.Framework;

public class QLearningTests
{
	[Test, AutoData]
	public void ShouldStopAfterNIterations(QLearningParameters learningParameters, int numberOfIterations)
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
	public void ShouldInitializeQValuesBeforeStart(QLearningParameters learningParameters)
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
