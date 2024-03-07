namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;

public class QValueReaderTests
{
	[Test, AutoMoqData]
	public async Task ShouldReadQValuesWith1BoardState(string storageFilePath,
		BoardActionQValues expectedBoardActionQValues,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueReader sut)
	{
		//Arrange
		var existingQValueCsv = new BoardActionQValuesStringConverter().GetLine(expectedBoardActionQValues);
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		using var stream = new MemoryStream(byteArray);
		fileSystemStub
			.Setup(stub => stub.FileExists(storageFilePath))
			.Returns(true);
		fileSystemStub
			.Setup(stub => stub.GetFileStreamToRead(storageFilePath))
			.Returns(stream);

		qLearningSystemConfigurationStub
			.SetupGet(stub => stub.QValueStorageFilePath)
			.Returns(storageFilePath);
		//Act
		var qValueTable = await sut.Read();
		//Assert
		var expectedBoard = new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 0, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			});
		var actualBoardActionQValues = qValueTable.ShouldHaveSingleItem();

		actualBoardActionQValues.ShouldBe(expectedBoardActionQValues, new BoardActionQValuesComparer());
	}

	[Test, AutoMoqData]
	public async Task ShouldReadQValuesWithMultipleBoardStates(string storageFilePath,
		BoardActionQValues[] expectedBoardActionQValues,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueReader sut)
	{
		//Arrange
		var existingQValueCsv = new BoardActionQValuesStringConverter().GetBoardQValueFileContent(expectedBoardActionQValues);
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		var stream = new MemoryStream(byteArray);
		fileSystemStub
			.Setup(stub => stub.FileExists(storageFilePath))
			.Returns(true);
		fileSystemStub
			.Setup(stub => stub.GetFileStreamToRead(storageFilePath))
			.Returns(stream);
			
		qLearningSystemConfigurationStub
			.SetupGet(stub => stub.QValueStorageFilePath)
			.Returns(storageFilePath);
		//Act
		var qValueTable = await sut.Read();
		//Assert
		qValueTable.ShouldBe(expectedBoardActionQValues, new BoardActionQValuesComparer());
	}

	[Test, AutoMoqData]
	public async Task ShouldReturnEmptyTable_WhenFileDoesntExist(string storageFilePath,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueReader sut)
	{
		//Arrange
		using var stream = new MemoryStream();
		fileSystemStub
			.Setup(stub => stub.FileExists(storageFilePath))
			.Returns(false);
			
		qLearningSystemConfigurationStub
			.SetupGet(stub => stub.QValueStorageFilePath)
			.Returns(storageFilePath);
		//Act
		var qValueTable = await sut.Read();
		//Assert
		qValueTable.ShouldBeEmpty();
	}
}
