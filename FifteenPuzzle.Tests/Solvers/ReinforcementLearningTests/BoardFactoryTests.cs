namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Solvers.ReinforcementLearning;
using global::AutoFixture.NUnit3;
using NUnit.Framework;
using Shouldly;

public class BoardFactoryTests
{
    [Test, AutoData]
    public void ShouldGetRandomBoard(BoardFactory sut) => sut.GetRandom().IsSolved.ShouldBeFalse();
}