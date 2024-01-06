namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using FluentAssertions;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

public class QLearningTests
{
	[Test, AutoMoqData]
	public async Task ShouldStopAfterNIterations(int numberOfIterationsExpected,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		QLearning sut)
	{
		//Arrange
		learningParametersStub
			.SetupGet(stub => stub.NumberOfIterations)
			.Returns(numberOfIterationsExpected);
		//Act
		await sut.Learn();
		//Assert
		sut.NumberOfIterations.Should().Be(numberOfIterationsExpected);
	}

	[Test, AutoMoqData]
	public async Task ShouldInitializeQValuesBeforeStart(int numberOfIterationsExpected,
		IEnumerable<BoardActionQValues> initialBoardActionQValues,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		QLearning sut)
	{
		//Arrange
		qValueReaderStub
			.Setup(stub => stub.Read(It.IsAny<Stream>()))
			.ReturnsAsync(initialBoardActionQValues);
		//Act
		await sut.Learn();
		//Assert
		//sut.NumberOfIterations.Should().Be(numberOfIterationsExpected);
	}
}
