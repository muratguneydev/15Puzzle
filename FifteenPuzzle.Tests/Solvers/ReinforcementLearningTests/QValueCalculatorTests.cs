namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Shouldly;

public class QValueCalculatorTests
{
	[Test, AutoMoqData]
	public void ShouldCalculate(double reward,
		BoardActionQValues[] boardActionQValues,
		QLearningHyperparameters qLearningHyperparametersToMimickInStub,
		[Frozen] [Mock] Mock<QLearningHyperparameters> qLearningHyperparametersStub,
		QValueCalculator sut)
    {
        //Arrange
        SetUpQLearningHyperparameters(qLearningHyperparametersToMimickInStub, qLearningHyperparametersStub);

        var selectedBoardActionQValues = boardActionQValues.First();
        var selectedBoard = selectedBoardActionQValues.Board;
        var selectedActionQValue = selectedBoardActionQValues.ActionQValues.First();

        var nextBoardActionQValuesForSelected = GetNextBoardActionQValues(selectedBoardActionQValues, selectedActionQValue);
        var expectedQValue = GetExpectedCalculatedQValue(nextBoardActionQValuesForSelected, selectedActionQValue,
            qLearningHyperparametersToMimickInStub, reward);
        var boardAction = new BoardAction(selectedBoard, selectedActionQValue, board => new Board(board));
        //Act
        var result = sut.Calculate(boardAction, nextBoardActionQValuesForSelected.ActionQValues, reward);
        //Assert
        result.ShouldBe(expectedQValue);
    }

    private static void SetUpQLearningHyperparameters(QLearningHyperparameters qLearningHyperparametersToMimickInStub, Mock<QLearningHyperparameters> qLearningHyperparametersStub)
    {
        qLearningHyperparametersStub
			.SetupGet(stub => stub.LearningRateAlpha)
			.Returns(qLearningHyperparametersToMimickInStub.LearningRateAlpha);
        qLearningHyperparametersStub
            .SetupGet(stub => stub.DiscountFactorGamma)
            .Returns(qLearningHyperparametersToMimickInStub.DiscountFactorGamma);
        qLearningHyperparametersStub
            .SetupGet(stub => stub.ExplorationProbabilityEpsilon)
            .Returns(qLearningHyperparametersToMimickInStub.ExplorationProbabilityEpsilon);
        qLearningHyperparametersStub
            .SetupGet(stub => stub.NumberOfIterations)
            .Returns(qLearningHyperparametersToMimickInStub.NumberOfIterations);
    }

    private static BoardActionQValues GetNextBoardActionQValues(BoardActionQValues selectedBoardActionQValues, ActionQValue selectedActionQValue)
	{
		var nextBoard = new Board(selectedBoardActionQValues.Board);
		nextBoard.Move(selectedActionQValue.Move.Number.ToString());
		
		var fixture = new Fixture();
		var nextActionQValues = nextBoard.GetMoves().Select(move => new ActionQValue(move, fixture.Create<double>()));
		return new BoardActionQValues(nextBoard, new ActionQValues(nextActionQValues));
	}

	private double GetExpectedCalculatedQValue(BoardActionQValues next, ActionQValue selectedAction,
		QLearningHyperparameters qLearningHyperparameters, double reward)
	{
		var maxNextQValue = next.Board.IsSolved ? 0 : next.ActionQValues.MaxQValue;
		var currentQValue = selectedAction.QValue;
		var learningRateMultiplier = reward + qLearningHyperparameters.DiscountFactorGamma * maxNextQValue - currentQValue;
		var newCurrentQValue = currentQValue
			+ qLearningHyperparameters.LearningRateAlpha * learningRateMultiplier;

		return newCurrentQValue;
	}
}