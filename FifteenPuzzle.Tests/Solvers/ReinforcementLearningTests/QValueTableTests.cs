namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture;
using NUnit.Framework;
using Shouldly;

public class QValueTableTests
{
	[Test, DomainAutoData]
	public void Should_ProvideAll0ActionQValues_WhenBoardNotFound(Board board,
		QValueTable sut)
	{
		//Act
		var result = sut.Get(board);
		//Assert
		result.All(actionQValue => actionQValue.QValue == 0).ShouldBeTrue();
	}

	[Test, DomainAutoData]
	public void Should_FindActionQValues_WhenBoardExists(BoardActionQValues[] boardActionQValues,
		QLearningHyperparameters qLearningHyperparameters)
	{
		//Arrange
		var sut = new QValueTable(boardActionQValues, qLearningHyperparameters);
		var expectedBoardActionQValues = boardActionQValues.Last();
		//Act
		var result = sut.Get(expectedBoardActionQValues.Board);
		//Assert
		result.ShouldBe(expectedBoardActionQValues.ActionQValues);
	}

	[Test, DomainAutoData]
	public void Should_UpdateActionQueueValues(BoardActionQValues[] boardActionQValues,
		QLearningHyperparameters qLearningHyperparameters,
		double reward)
	{
		//Arrange
		var selectedBoardActionQValues = boardActionQValues.First();
		
		var selectedBoard = selectedBoardActionQValues.Board;
		var selectedActionQValue = selectedBoardActionQValues.ActionQValues.First();

		var nextBoardActionQValuesForSelected = GetNextBoardActionQValues(selectedBoardActionQValues, selectedActionQValue);
		var boardActionQValuesWithNext = boardActionQValues.Append(nextBoardActionQValuesForSelected);
		var sut = new QValueTable(boardActionQValuesWithNext, qLearningHyperparameters);
		
		//this has to be done before Act
		var expectedQValue = GetExpectedCalculatedQValue(nextBoardActionQValuesForSelected, selectedActionQValue,
			qLearningHyperparameters, reward);
		//Act
		var boardAction = new BoardAction(selectedBoard, selectedActionQValue, board => new Board(board));
		sut.UpdateQValues(boardAction, reward);
		//Assert
		selectedActionQValue.QValue.ShouldBe(expectedQValue);
	}

	[Test, DomainAutoData]
	public void ShouldProvideEmptyQValueTable(QLearningHyperparameters qLearningHyperparameters)
	{
		QValueTable.Empty(qLearningHyperparameters).ShouldBeEmpty();
	}

	private double GetExpectedCalculatedQValue(BoardActionQValues next, ActionQValue selectedAction,
		QLearningHyperparameters qLearningHyperparameters, double reward)
	{
		var maxNextQValue = next.Board.IsSolved ? 0 : next.ActionQValues.MaxQValue;
		var currentQValue = selectedAction.QValue;
		var learningRateMultiplier = reward + qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
		var newCurrentQValue = currentQValue
			+ qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;

		return newCurrentQValue;
	}

	private static BoardActionQValues GetNextBoardActionQValues(BoardActionQValues selectedBoardActionQValues, ActionQValue selectedActionQValue)
	{
		var nextBoard = new Board(selectedBoardActionQValues.Board);
		nextBoard.Move(selectedActionQValue.Move.Number.ToString());
		
		var fixture = new Fixture();
		var nextMovableCells = nextBoard.GetMovableCells();
		var nextActionQValues = nextMovableCells.Select(cell => new ActionQValue(new Move(int.Parse(cell.Value)), fixture.Create<double>()));
		return new BoardActionQValues(nextBoard, new ActionQValues(nextActionQValues));
	}
}
