namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common.AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Shouldly;

public class QValueReaderTests
{
	private const char Separator = ',';
	private const char ActionQValueSeparator = '/';

	[Test, AutoMoqData]
	public async Task ShouldReadQValuesWith1BoardState(string storageFilePath,
		ActionQValues expectedActionQValues,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueReader sut)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues)}";
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

		var actualBoard = actualBoardActionQValues.Board;
		BoardAsserter.ShouldBeEquivalent(expectedBoard, actualBoard);

		var actualActionQValues = actualBoardActionQValues.ActionQValues;
		actualActionQValues.Should().BeEquivalentTo(expectedActionQValues);
	}

	[Test, AutoMoqData]
	public async Task ShouldReadQValuesWithMultipleBoardStates(string storageFilePath,
		ActionQValues[] expectedActionQValues,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueReader sut)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[0])}
1,2,3,,5,4,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[1])}
1,2,3,4,5,8,7,,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[2])}";
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
		qValueTable.Should().HaveCount(3);

		var expectedBoards = new[] {
			new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 0, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			}),
			new Board(new[,]
			{
				{ 1, 2, 3, 0 },
				{ 5, 4, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			}),
			new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 8, 7, 0 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			}),
		};
		BoardAsserter.ShouldBeEquivalent(expectedBoards, qValueTable.Select(boardActionQValue => boardActionQValue.Board));

		var actualActionQValues = qValueTable.Select(boardActionQValue => boardActionQValue.ActionQValues);
		actualActionQValues.Should().BeEquivalentTo(expectedActionQValues);
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

	private static string GetActionQValuesString(ActionQValues actionQValues) =>
		string.Join(Separator, actionQValues.Select(a => $"{a.Move.Number}{ActionQValueSeparator}{a.QValue}"));
}
