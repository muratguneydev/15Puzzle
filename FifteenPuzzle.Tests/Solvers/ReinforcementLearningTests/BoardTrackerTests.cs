namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using NUnit.Framework;
using Shouldly;

public class BoardTrackerTests
{
	[Test, DomainAutoData]
	public void ShouldTrack_WhenAddedBoardMove(Board board,
		BoardTracker sut)
	{
		//Act & Assert
		sut.Add(board);
		//Assert
		sut.WasProcessedBefore(board).ShouldBeTrue();
	}

	[Test, DomainAutoData]
	public void ShouldBecomeEmpty_WhenCleared(List<Board> boards,
		BoardTracker sut)
	{
		//Arrange
		boards.ForEach(sut.Add);
		//Act
		sut.Clear();
		//Assert
		boards.Any(sut.WasProcessedBefore).ShouldBeFalse();
	}
}