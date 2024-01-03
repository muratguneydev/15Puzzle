namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FluentAssertions;
using NUnit.Framework;

public class QValueWriterTests
{
	[Test, AutoData]
	public async Task ShouldWriteQValues(ActionQValues[] expectedActionQValues,
		BoardActionQValuesStringConverter boardActionQValuesStringConverter)
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

		var expectedQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{expectedActionQValues[0].Up},{expectedActionQValues[0].Right},{expectedActionQValues[0].Down},{expectedActionQValues[0].Left}
1,2,3,,5,4,7,8,9,6,11,12,13,14,15,10,{expectedActionQValues[1].Up},{expectedActionQValues[1].Right},{expectedActionQValues[1].Down},{expectedActionQValues[1].Left}
1,2,3,4,5,8,7,,9,6,11,12,13,14,15,10,{expectedActionQValues[2].Up},{expectedActionQValues[2].Right},{expectedActionQValues[2].Down},{expectedActionQValues[2].Left}";
		
		var qValueTable = new QValueTable(new[] {
			new BoardActionQValues(expectedBoards[0], expectedActionQValues[0]),
			new BoardActionQValues(expectedBoards[1], expectedActionQValues[1]),
			new BoardActionQValues(expectedBoards[2], expectedActionQValues[2])
		});

		using var stream = new MemoryStream();
		var sut = new QValueWriter(boardActionQValuesStringConverter, stream);
		//Act
		await sut.Write(qValueTable);
		//Assert
		stream.Seek(0, SeekOrigin.Begin);
		using var reader = new StreamReader(stream, Encoding.UTF8);
		var writtenText = await reader.ReadToEndAsync();
		
		sut.Dispose();//we need the stream until this point. Disposing the StreamWriter will also dispose the stream.
		writtenText.Should().Be(expectedQValueCsv);
	}
}