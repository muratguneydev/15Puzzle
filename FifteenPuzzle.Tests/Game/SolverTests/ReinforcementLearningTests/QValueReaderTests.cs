namespace FifteenPuzzle.Tests.Game.SolverTests.ReinforcementLearningTests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Game.Solvers.ReinforcementLearning;
using FluentAssertions;
using NUnit.Framework;

public class QValueReaderTests
{
	[Test, AutoData]
	public void ShouldReadQValuesWith1BoardState(ActionQValues expectedActionQValues)
	{
		//Arrange
		var existingQValueCsv = @$"1,2,3,4,5,,7,8,9,6,11,12,13,14,15,10,{expectedActionQValues.Up},{expectedActionQValues.Right},{expectedActionQValues.Down},{expectedActionQValues.Left}";
		var byteArray = Encoding.UTF8.GetBytes(existingQValueCsv);
		var stream = new MemoryStream(byteArray);
		var qValueReader = new QValueReader(stream);
		//Act
		var qValueTable = qValueReader.Read();
		//Assert
		qValueTable.Should().HaveCount(1);

		var expectedBoard = new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 0, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			});
		var actualBoardActionQValues = qValueTable.Single();

		var actualBoard = actualBoardActionQValues.Board;
		actualBoard.Cells.Should().BeEquivalentTo(expectedBoard.Cells);
		actualBoard.Rows.Should().BeEquivalentTo(expectedBoard.Rows);

		var actualActionQValues = actualBoardActionQValues.ActionQValues;
		actualActionQValues.Should().Be(expectedActionQValues);
	}
}