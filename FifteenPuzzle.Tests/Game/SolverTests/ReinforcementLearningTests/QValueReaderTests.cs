namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FluentAssertions;
using NUnit.Framework;
using Shouldly;

public class QValueReaderTests
{
	[Test, AutoData]
	public async Task ShouldReadQValuesWith1BoardState(ActionQValues expectedActionQValues,
		BoardActionQValuesStringConverter boardActionQValuesStringConverter)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{expectedActionQValues.Up},{expectedActionQValues.Right},{expectedActionQValues.Down},{expectedActionQValues.Left}";
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		var stream = new MemoryStream(byteArray);
		var sut = new QValueReader(boardActionQValuesStringConverter, stream);
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
		actualActionQValues.Should().Be(expectedActionQValues);
	}

	[Test, AutoData]
	public async Task ShouldReadQValuesWithMultipleBoardStates(ActionQValues[] expectedActionQValues,
		BoardActionQValuesStringConverter boardActionQValuesStringConverter)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{expectedActionQValues[0].Up},{expectedActionQValues[0].Right},{expectedActionQValues[0].Down},{expectedActionQValues[0].Left}
1,2,3,,5,4,7,8,9,6,11,12,13,14,15,10,{expectedActionQValues[1].Up},{expectedActionQValues[1].Right},{expectedActionQValues[1].Down},{expectedActionQValues[1].Left}
1,2,3,4,5,8,7,,9,6,11,12,13,14,15,10,{expectedActionQValues[2].Up},{expectedActionQValues[2].Right},{expectedActionQValues[2].Down},{expectedActionQValues[2].Left}";
		
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		var stream = new MemoryStream(byteArray);
		var sut = new QValueReader(boardActionQValuesStringConverter, stream);
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
}
