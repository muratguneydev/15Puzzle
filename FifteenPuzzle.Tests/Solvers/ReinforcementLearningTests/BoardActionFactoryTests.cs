namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture.NUnit3;
using NUnit.Framework;
using Shouldly;

public class BoardActionFactoryTests
{
	[Test, AutoMoqData]
	public void ShouldProvideBoardAction(Board board,
		ActionQValue action,
		[Frozen] BoardFactory boardFactoryStub,
		BoardActionFactory sut)
	{
		//Act
		var boardAction = sut.Get(board, action);
		//Assert
		BoardActionAsserter.ShouldBeEquivalent(boardAction, new BoardAction(board, action, boardFactoryStub.Clone));
	}

	[Test, AutoMoqData]
	public void ShouldProvideDefaultBoardAction([Frozen] BoardFactory boardFactoryStub,
		BoardActionFactory sut)
	{
		//Act
		var result = sut.GetDefault();
		//Assert
		var expectedBoard = Board.Solved;
		var expectedAction = new ActionQValue(expectedBoard.GetMoves().First(), default);
		var expected = new BoardAction(expectedBoard, expectedAction, boardFactoryStub.Clone);
		BoardActionAsserter.ShouldBeEquivalent(expected, result);
	}
}