namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using NUnit.Framework;
using Shouldly;

public class GreedyManhattanDistanceRewardStrategyTests
{
	[Test, DomainAutoData]
	public void ShouldCalculateReward(GreedyManhattanDistanceRewardStrategy sut)
	{
		//Arrange
		var board = new Board(new[,]
			{
				{ 1, 2, 3, 4 },
				{ 5, 0, 7, 8 },
				{ 9, 6, 11, 12 },
				{ 13, 14, 15, 10 }
			});
		//Act
		var reward = sut.Calculate(board);
		//Assert
		var tenToItsPosition = -3;//2 left, 1 up
		var sixToItsPosition = -1;//1 up

		reward.ShouldBe(tenToItsPosition + sixToItsPosition);
	}
}