namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common.AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;

public class QValueWriterTests
{
	private const char Separator = ',';
	private const char ActionQValueSeparator = '/';

	[Test, AutoMoqData]
	public async Task ShouldWriteQValues(string storageFilePath,
		ActionQValues[] expectedActionQValues,
		[Frozen] [Mock] Mock<FileSystem> fileSystemStub,
		[Frozen] [Mock] Mock<QLearningSystemConfiguration> qLearningSystemConfigurationStub,
		QValueWriter sut)
	{
		//Arrange
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

		var expectedQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[0])}
1,2,3,,5,4,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[1])}
1,2,3,4,5,8,7,,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[2])}";
		
		var qValueTable = new QValueTable(new[] {
			new BoardActionQValues(expectedBoards[0], expectedActionQValues[0]),
			new BoardActionQValues(expectedBoards[1], expectedActionQValues[1]),
			new BoardActionQValues(expectedBoards[2], expectedActionQValues[2])
		});

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

	private static string GetActionQValuesString(ActionQValues actionQValues) =>
		string.Join(Separator, actionQValues.Select(a => $"{a.Move.Number}{ActionQValueSeparator}{a.QValue}"));
}
