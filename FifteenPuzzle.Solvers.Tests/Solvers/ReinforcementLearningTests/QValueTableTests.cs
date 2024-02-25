namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using AutoFixture;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common.AutoFixture;
using NUnit.Framework;
using Shouldly;

public class QValueTableTests
{
	[Test, DomainAutoData]
	public void Should_ProvideAll0ActionQValues_WhenBoardNotFound(Board board,
		QValueTable sut)
	{
		//Act
		var result = sut.GetOrAddDefaultActions(board);
		//Assert
		result.All(actionQValue => actionQValue.QValue == 0).ShouldBeTrue();
	}

	[Test, DomainAutoData]
	public void Should_CreateAndStoreAll0ActionQValues_WhenBoardNotFound(Board board,
		double reward,
		QValueTable sut)
	{
		//Arrange
		var actions = sut.GetOrAddDefaultActions(board);
		var selectedAction = actions.First();
		var boardAction = new BoardAction(board, selectedAction, brd => new Board(brd));
		sut.UpdateQValues(boardAction, reward);
		//Act
		var result = sut.GetOrAddDefaultActions(board);
		//Assert
		result.Get(selectedAction).QValue.ShouldBe(reward);
	}

	[Test, DomainAutoData]
	public void Should_FindActionQValues_WhenBoardExists(BoardActionQValues[] boardActionQValues)
	{
		//Arrange
		var sut = new QValueTable(boardActionQValues);
		var expectedBoardActionQValues = boardActionQValues.Last();
		//Act
		var result = sut.GetOrAddDefaultActions(expectedBoardActionQValues.Board);
		//Assert
		result.ShouldBe(expectedBoardActionQValues.ActionQValues);
	}

	[Test, DomainAutoData]
	public void Should_UpdateActionQueueValues(BoardActionQValues[] boardActionQValues, double reward)
	{
		//Arrange
		var selectedBoardActionQValues = boardActionQValues.First();
		
		var selectedBoard = selectedBoardActionQValues.Board;
		var selectedActionQValue = selectedBoardActionQValues.ActionQValues.First();

		var nextBoardActionQValuesForSelected = GetNextBoardActionQValues(selectedBoardActionQValues, selectedActionQValue);
		var boardActionQValuesWithNext = boardActionQValues.Append(nextBoardActionQValuesForSelected);
		var sut = new QValueTable(boardActionQValuesWithNext);		
		//Act
		var boardAction = new BoardAction(selectedBoard, selectedActionQValue, board => new Board(board));
		sut.UpdateQValues(boardAction, reward);
		var actions = sut.GetOrAddDefaultActions(selectedBoard);
		//Assert
		selectedActionQValue.QValue.ShouldBe(reward);

		var actualActionQValueFromBoard = actions.Single(action => action.Move == selectedActionQValue.Move);
		actualActionQValueFromBoard.QValue.ShouldBe(reward);
	}

	[Test, DomainAutoData]
	public void ShouldProvideEmptyQValueTable(QLearningHyperparameters qLearningHyperparameters)
	{
		QValueTable.Empty(qLearningHyperparameters).ShouldBeEmpty();
	}

	private static BoardActionQValues GetNextBoardActionQValues(BoardActionQValues selectedBoardActionQValues, ActionQValue selectedActionQValue)
	{
		var nextBoard = new Board(selectedBoardActionQValues.Board);
		nextBoard.Move(selectedActionQValue.Move.Number.ToString());
		
		var fixture = new Fixture();
		var nextActionQValues = nextBoard.GetMoves().Select(move => new ActionQValue(move, fixture.Create<double>()));
		return new BoardActionQValues(nextBoard, new ActionQValues(nextActionQValues));
	}
}
