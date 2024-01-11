namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests.ActionSelectionTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

public class NonRepeatingActionSelectionPolicyTests
{
	[Test, AutoMoqData]
	public void ShouldNotRepeatTheSameBoard(Board currentBoard,
		Mock<IActionSelectionPolicy> actionSelectionPolicyStub,
		[Frozen] Mock<IActionSelectionPolicyFactory> actionSelectionPolicyFactoryStub,
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
		var boardTracker = new HashSet<Board>(new BoardComparer()) { dupeBoard.NextBoard };

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
		var result = sut.PickAction(actionQValues, currentBoard, boardTracker);
		//Assert
		BoardActionAsserter.ShouldBeEquivalent(expectedBoardAction, result);
	}
}