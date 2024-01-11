namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using NUnit.Framework;
using Shouldly;

public class BoardActionTests
{
	[Test, DomainAutoData]
	public void ShouldPopulateNextBoard(BoardActionQValues boardActions, BoardFactory boardFactory)
	{
		var boardAction = new BoardAction(boardActions.Board, boardActions.ActionQValues.First(), boardFactory.Clone);
		
		boardAction.Board.ShouldBe(boardActions.Board);
		boardAction.ActionQValue.ShouldBe(boardActions.ActionQValues.First());

		var expectedNextBoard = boardFactory.Clone(boardActions.Board);
		expectedNextBoard.Move(boardActions.ActionQValues.First().Move.Number.ToString());
		BoardAsserter.ShouldBeEquivalent(expectedNextBoard, boardAction.NextBoard);
	}
}
