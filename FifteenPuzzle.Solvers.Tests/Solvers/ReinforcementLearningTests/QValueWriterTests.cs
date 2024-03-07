namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common.AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;

public class QValueWriterTests
{
	[Test, AutoMoqData]
	public async Task ShouldWriteQValues(string storageFilePath,
		BoardActionQValues[] expectedBoardActionQValues,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueWriter sut)
	{
		//Arrange
		var expectedQValueCsv = new BoardActionQValuesStringConverter().GetBoardQValueFileContent(expectedBoardActionQValues);
		var qValueTable = new QValueTable(expectedBoardActionQValues);

		using var stream = new MemoryStream();
		fileSystemStub
			.Setup(stub => stub.GetFileStreamToWrite(storageFilePath))
			.Returns(stream);
		qLearningSystemConfigurationStub
			.SetupGet(stub => stub.QValueStorageFilePath)
			.Returns(storageFilePath);
		//Act
		var toDispose = await sut.Write(qValueTable);
		//Assert
		stream.Seek(0, SeekOrigin.Begin);
		using var reader = new StreamReader(stream, Encoding.UTF8);
		var writtenText = await reader.ReadToEndAsync();
		toDispose.Dispose();
		
		//sut.Dispose();//we need the stream until this point. Disposing the StreamWriter will also dispose the stream.
		writtenText.Should().Be(expectedQValueCsv);
	}
}
