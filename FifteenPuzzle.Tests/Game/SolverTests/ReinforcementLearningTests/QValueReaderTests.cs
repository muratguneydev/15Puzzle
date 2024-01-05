namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

using System.Text;
using FifteenPuzzle.Game;
using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using Shouldly;

public class QValueReaderTests
{
	private const char Separator = ',';
	private const char ActionQValueSeparator = '/';

	[Test, DomainAutoData]
	public async Task ShouldReadQValuesWith1BoardState(ActionQValues expectedActionQValues,
		BoardActionQValuesStringConverter boardActionQValuesStringConverter)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues)}";
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		var stream = new MemoryStream(byteArray);
		var sut = new QValueReader(boardActionQValuesStringConverter);
		//Act
		var qValueTable = await sut.Read(stream);
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

	[Test, DomainAutoData]
	public async Task ShouldReadQValuesWithMultipleBoardStates(ActionQValues[] expectedActionQValues,
		BoardActionQValuesStringConverter boardActionQValuesStringConverter)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[0])}
1,2,3,,5,4,7,8,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[1])}
1,2,3,4,5,8,7,,9,6,11,12,13,14,15,10,{GetActionQValuesString(expectedActionQValues[2])}";
		
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		var stream = new MemoryStream(byteArray);
		var sut = new QValueReader(boardActionQValuesStringConverter);
		//Act
		var qValueTable = await sut.Read(stream);
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

	[Test, DomainAutoData]
	public async Task ShouldReturnEmptyTableWhenFileDoesntExist(
		BoardActionQValuesStringConverter boardActionQValuesStringConverter)
	{
		//Arrange
		var stream = new MemoryStream();
		var sut = new QValueReader(boardActionQValuesStringConverter);
		//Act
		var qValueTable = await sut.Read(stream);
		//Assert
		qValueTable.ShouldBeEmpty();
	}

	private static string GetActionQValuesString(ActionQValues actionQValues) =>
		string.Join(Separator, actionQValues.Select(a => $"{a.Move.Number}{ActionQValueSeparator}{a.QValue}"));
}
