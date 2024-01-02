namespace FifteenPuzzle.Tests.Game.SolverTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Game.Solvers;
using FluentAssertions;
using NUnit.Framework;

public class DepthFirstSolverTests
{
	[Test]
	public void ShouldSolve_WhenAlreadySolved()
	{
		//Arrange
		var solver = new DepthFirstSolver(_ => {});
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
		var solver = new DepthFirstSolver(_ => {});
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
}