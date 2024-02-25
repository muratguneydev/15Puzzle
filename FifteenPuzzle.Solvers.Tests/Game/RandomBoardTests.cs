namespace FifteenPuzzle.Tests.Game;

using FifteenPuzzle.Game;
using global::AutoFixture.NUnit3;
using NUnit.Framework;
using Shouldly;

public class RandomBoardTests
{
    [Test, AutoData]
    public void ShouldBeDifferentThanSolved(RandomBoard sut, BoardComparer boardComparer) =>
		boardComparer.Equals(sut, Board.Solved).ShouldBeFalse();
}
