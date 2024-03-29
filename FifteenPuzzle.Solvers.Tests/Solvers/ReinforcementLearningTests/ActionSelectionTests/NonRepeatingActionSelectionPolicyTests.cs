namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests.ActionSelectionTests;

using AutoFixture;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Moq;
using NUnit.Framework;
using Shouldly;

public class NonRepeatingActionSelectionPolicyTests
{
	[Test, AutoMoqData]
	public void ShouldNotRepeatTheSameBoard(Board currentBoard,
		Mock<IActionSelectionPolicy> actionSelectionPolicyStub,
		[Frozen] Mock<IActionSelectionPolicyFactory> actionSelectionPolicyFactoryStub,
		[Frozen] [Mock] Mock<BoardTracker> boardTrackerStub,
		NonRepeatingActionSelectionPolicy sut)
	{
		//Arrange
		var fixture = AutoFixtureFactory.Create();
		var actions = currentBoard
			.GetMoves()
			.Select(move => new ActionQValue(move, fixture.Create<double>()));
		
		var actionQValues = new ActionQValues(actions);
		var selectedActionLeadingToDupeBoard = actionQValues.First();
		
		var actionsWithoutActionLeadingToDupeBoard = actionQValues.Remove(selectedActionLeadingToDupeBoard);
		var actionLeadingToNonDupeBoard = actionsWithoutActionLeadingToDupeBoard.First();

		var dupeBoard = new BoardAction(currentBoard, selectedActionLeadingToDupeBoard, board => new Board(board));
		boardTrackerStub
			.Setup(stub => stub.WasProcessedBefore(It.IsAny<Board>()))
			.Returns((Board board) => new BoardComparer().Equals(board, dupeBoard.NextBoard));

		var expectedBoardAction = new BoardAction(currentBoard, actionLeadingToNonDupeBoard, board => new Board(board));
		
		actionSelectionPolicyStub
			.Setup(stub => stub.PickAction(actionQValues))
			.Returns(selectedActionLeadingToDupeBoard);
		actionSelectionPolicyStub
			.Setup(stub => stub.PickAction(actionsWithoutActionLeadingToDupeBoard))
			.Returns(actionLeadingToNonDupeBoard);
		actionSelectionPolicyFactoryStub
			.Setup(stub => stub.Get())
			.Returns(actionSelectionPolicyStub.Object);
        //Act
        var (isSuccessful, boardAction) = sut.TryPickAction(actionQValues, currentBoard);
        //Assert
		isSuccessful.ShouldBeTrue();
        BoardActionAsserter.ShouldBeEquivalent(expectedBoardAction, boardAction);
	}

	[Test, AutoMoqData]
	public void ShouldIndicateNotFound_WhenCantFindAnyNewBoardActionPair(BoardActionQValues boardActionQValues,
		Mock<IActionSelectionPolicy> actionSelectionPolicyStubAlwaysPickingTheFirst,
		[Frozen] Mock<IActionSelectionPolicyFactory> actionSelectionPolicyFactoryStub,
		[Frozen] [Mock] Mock<BoardTracker> boardTrackerStubAllBoardsProcessed,
		NonRepeatingActionSelectionPolicy sut)
	{
		//Arrange
		var currentBoard = boardActionQValues.Board;
		var actionQValues = boardActionQValues.ActionQValues;
		
		var allPossibleNextBoards = actionQValues
			.Select(action => new BoardAction(currentBoard, action, board => new Board(board)))
			.Select(boardAction => boardAction.NextBoard)
			.ToHashSet(new BoardComparer());
		//Note: We could return "true" for any input but wanted to make sure that the stub method is called with the correct parmaeters.
		boardTrackerStubAllBoardsProcessed
			.Setup(stub => stub.WasProcessedBefore(It.IsAny<Board>()))
			.Returns((Board board) => allPossibleNextBoards.Contains(board));
		
		actionSelectionPolicyStubAlwaysPickingTheFirst
			.Setup(stub => stub.PickAction(It.IsAny<ActionQValues>()))
			.Returns((ActionQValues actions) => actions.First());
		actionSelectionPolicyFactoryStub
			.Setup(stub => stub.Get())
			.Returns(actionSelectionPolicyStubAlwaysPickingTheFirst.Object);
		//Act
        var (isSuccessful, boardAction) = sut.TryPickAction(actionQValues, currentBoard);
        //Assert
		isSuccessful.ShouldBeFalse();
	}
}
