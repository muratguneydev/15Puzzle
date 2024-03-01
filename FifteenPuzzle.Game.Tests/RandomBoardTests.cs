namespace FifteenPuzzle.Tests.Game;

using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using NUnit.Framework;
using Shouldly;

public class RandomBoardTests
{
    [Test, AutoData]
    public void ShouldBeDifferentThanSolved(RandomBoard sut, BoardComparer boardComparer) =>
		boardComparer.Equals(sut, Board.Solved).ShouldBeFalse();
}
