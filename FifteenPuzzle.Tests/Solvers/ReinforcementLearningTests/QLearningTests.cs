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
		IEnumerable<BoardActionQValues> someBoardActionQValues,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen] [Mock] Mock<QValueWriter> qValueWriterDummy,
		QLearning sut)
	{
		//Arrange
		learningParametersStub
			.SetupGet(stub => stub.NumberOfIterations)
			.Returns(numberOfIterationsExpected);
		qValueReaderStub
			.Setup(stub => stub.Read())
			.ReturnsAsync(someBoardActionQValues);
		qValueWriterDummy
			.Setup(dummy => dummy.Write(It.IsAny<QValueTable>()))
			.Returns(Task.CompletedTask);
		//Act
		await sut.Learn();
		//Assert
		sut.NumberOfIterations.Should().Be(numberOfIterationsExpected);
	}

	[Test, AutoMoqData]
	public async Task ShouldInitializeQValuesBeforeStart(
		IEnumerable<BoardActionQValues> initialBoardActionQValues,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersMock,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen] [Mock] Mock<QValueWriter> qValueWriterSpy,
		QLearning sut)
	{
		//Arrange
		qValueReaderStub
			.Setup(stub => stub.Read())
			.ReturnsAsync(initialBoardActionQValues);
		qValueWriterSpy
			.Setup(dummy => dummy.Write(It.IsAny<QValueTable>()))
			.Returns(Task.CompletedTask);
		//Act
		await sut.Learn();
		//Assert
		var expectedQValueTable = new QValueTable(initialBoardActionQValues, learningParametersMock.Object);
		qValueWriterSpy.Verify(spy => spy.Write(expectedQValueTable));
	}
}
