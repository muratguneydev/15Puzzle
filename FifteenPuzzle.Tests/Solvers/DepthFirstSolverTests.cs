namespace FifteenPuzzle.Tests.SolverTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers;
using FluentAssertions;
using NUnit.Framework;

public class DepthFirstSolverTests
{
	private static readonly BoardComparer BoardComparer = new();

	[Test]
	public void ShouldSolve_WhenAlreadySolved()
	{
		//Arrange
		var solver = new DepthFirstSolver();
		//Act
		solver.Solve(Board.Solved);
		//Assert
		solver.History.Should().HaveCount(1);
		solver.History.Single().Should().BeEquivalentTo(Board.Solved);
	}

	[Test]
	public void ShouldSolve()
	{
		//Arrange
		var solver = new DepthFirstSolver();
		var board = new Board(new[,]
		{
			{ 1, 2, 3, 4 },
			{ 5, 6, 7, 8 },
			{ 9, 10, 0, 11 },
			{ 13, 14, 15, 12 }
		});
		//Act
		solver.Solve(board);
		//Assert
		solver.History.Should().HaveCount(3);
		solver.History.Last().Should().BeEquivalentTo(Board.Solved);
	}

	[Test]
	public void ShouldRaiseEvent_WhenNewBoardIsTested()
    {
        //Arrange
        var solver = new DepthFirstSolver();
        var testedBoards = new List<Board>();
        solver.AddOnNewItemTested(testedBoards.Add);
        var currentBoard = new Board(new[,]
        {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 0, 11 },
            { 13, 14, 15, 12 }
        });
        //Act
        solver.Solve(currentBoard);
        //Assert
        GetBoardHashSet(solver.History).SetEquals(GetBoardHashSet(testedBoards));
    }

    private static HashSet<Board> GetBoardHashSet(IEnumerable<Board> boards) => new HashSet<Board>(boards, BoardComparer);
}
