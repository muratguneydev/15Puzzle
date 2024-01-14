namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests.ActionSelectionTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture;
using global::AutoFixture.NUnit3;
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
			.GetMovableCells()
			.Select(cell => new ActionQValue(new Move(int.Parse(cell.Value)), fixture.Create<double>()));
		
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
		var result = sut.PickAction(actionQValues, currentBoard);
		//Assert
		BoardActionAsserter.ShouldBeEquivalent(expectedBoardAction, result);
	}

	[Test, AutoMoqData]
	public void ShouldThrow_WhenCantFindAnyActionsLeadingToANewBoard(BoardActionQValues boardActionQValues,
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
		boardTrackerStubAllBoardsProcessed
			.Setup(stub => stub.WasProcessedBefore(It.IsAny<Board>()))
			.Returns((Board board) => allPossibleNextBoards.Contains(board));
		
		actionSelectionPolicyStubAlwaysPickingTheFirst
			.Setup(stub => stub.PickAction(actionQValues))
			.Returns((ActionQValues actionQValues) => actionQValues.First());
		actionSelectionPolicyFactoryStub
			.Setup(stub => stub.Get())
			.Returns(actionSelectionPolicyStubAlwaysPickingTheFirst.Object);
		//Act & Assert
		Should.Throw<Exception>(() => sut.PickAction(actionQValues, currentBoard));
	}
}
