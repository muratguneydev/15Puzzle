namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using Shouldly;

public class BoardActionAsserter
{
    public static void ShouldBeEquivalent(BoardAction expected, BoardAction actual) =>
		new BoardActionComparer().Equals(expected, actual).ShouldBeTrue();
}
