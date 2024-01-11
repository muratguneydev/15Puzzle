namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using Shouldly;

public class BoardActionAsserter
{
    public static void ShouldBeEquivalent(BoardAction expected, BoardAction actual)
    {
		BoardAsserter.ShouldBeEquivalent(expected.Board, actual.Board);
		BoardAsserter.ShouldBeEquivalent(expected.NextBoard, actual.NextBoard);
		actual.ActionQValue.ShouldBe(expected.ActionQValue);
        
    }
}