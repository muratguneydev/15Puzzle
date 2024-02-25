namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Shouldly;

public class BoardFactoryTests
{
    [Test, AutoData]
    public void ShouldGetRandomBoard(BoardFactory sut) => sut.GetRandom().IsSolved.ShouldBeFalse();
    public void ShouldGetSolvedBoard(BoardFactory sut) => sut.GetSolved().IsSolved.ShouldBeTrue();

	[Test, AutoData]
    public void ShouldGetSolvableBoard(Mock<BoardFactory> sutMock)
	{
		//Arrange
		var solvableBoardStub = new Mock<Board>(Board.Solved);
		solvableBoardStub
			.SetupGet(stub => stub.IsSolvable)
			.Returns(true);
		sutMock
			.Setup(stub => stub.GetRandom())
			.Returns(solvableBoardStub.Object);
		//Act
		var solvableBoard = sutMock.Object.GetSolvable();
		//Assert
		solvableBoard.IsSolvable.ShouldBeTrue();
	}
}
