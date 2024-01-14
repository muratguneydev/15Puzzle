namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.AutoFixture;
using FluentAssertions;
using global::AutoFixture;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Shouldly;

public class QLearningTests
{
	private static readonly Random Random = new();

	[Test, AutoMoqData]
	public async Task ShouldStopAfterNIterations(
		IEnumerable<BoardActionQValues> someBoardActionQValues,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen] [Mock] Mock<QValueWriter> qValueWriterDummy,
		[Frozen] [Mock] Mock<BoardFactory> boardFactoryStub,
		[Frozen] [Mock] Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
		QLearning sut)
	{
		//Arrange
		SetUpQLearningHyperparameters(learningParametersStub);
        SetUpQValueReader(qValueReaderStub, someBoardActionQValues);
        SetUpQValueWriter(qValueWriterDummy);

		const int numberOfMovesTillResolved = 3;
		var numberOfIterations = learningParametersStub.Object.NumberOfIterations;
        var selectedBoardActionQValues = someBoardActionQValues.First();

        SetUpActionSelectionPolicy(actionSelectionPolicyStub, numberOfMovesTillResolved, numberOfIterations, selectedBoardActionQValues);
        SetUpBoardFactory(boardFactoryStub, selectedBoardActionQValues.Board);

		var iterationCount = 0;
		sut.OnIterationCompleted += numberOfIterations => iterationCount = numberOfIterations;
		//Act
		await sut.Learn();
		//Assert
		iterationCount.Should().Be(numberOfIterations);
	}

	[Test, AutoMoqData]
	public async Task Should_LoadExistingQValuesAtStart_AndPersistAtTheEnd(
		IEnumerable<BoardActionQValues> boardActionQValues,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen] [Mock] Mock<QValueWriter> qValueWriterSpy,
		[Frozen] [Mock] Mock<BoardFactory> boardFactoryStub,
		[Frozen] [Mock] Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
		QLearning sut)
	{
		//Arrange
		SetUpQLearningHyperparameters(learningParametersStub);
        SetUpQValueReader(qValueReaderStub, boardActionQValues);
        SetUpQValueWriter(qValueWriterSpy);

		const int numberOfMovesTillResolved = 3;
		var numberOfIterations = learningParametersStub.Object.NumberOfIterations;
        var selectedBoardActionQValues = boardActionQValues.First();

        SetUpActionSelectionPolicy(actionSelectionPolicyStub, numberOfMovesTillResolved, numberOfIterations, selectedBoardActionQValues);
        SetUpBoardFactory(boardFactoryStub, selectedBoardActionQValues.Board);
		//Act
		await sut.Learn();
		//Assert
		//Note: boardActionQValues gets updated during the learning.
		var expectedQValueTable = new QValueTable(boardActionQValues, learningParametersStub.Object);
		qValueWriterSpy.Verify(spy => spy.Write(expectedQValueTable));
	}

	[Test, AutoMoqData]
	public async Task Should_UpdateQValuesUntilBoardIsResolved(
		IEnumerable<BoardActionQValues> boardActionQValues,
		[Frozen] [Mock] Mock<QLearningHyperparameters> learningParametersStub,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen] [Mock] Mock<QValueWriter> qValueWriterSpy,
		[Frozen] [Mock] Mock<BoardFactory> boardFactoryStub,
		[Frozen] [Mock] Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
		QLearning sut)
    {
        //Arrange
        SetUpQLearningHyperparameters(learningParametersStub);
        SetUpQValueReader(qValueReaderStub, boardActionQValues);
        SetUpQValueWriter(qValueWriterSpy);

        const int numberOfMovesTillResolved = 3;
		var numberOfIterations = learningParametersStub.Object.NumberOfIterations;
        var selectedBoardActionQValues = boardActionQValues.First();

		SetUpActionSelectionPolicy(actionSelectionPolicyStub, numberOfMovesTillResolved, numberOfIterations, selectedBoardActionQValues);
        SetUpBoardFactory(boardFactoryStub, selectedBoardActionQValues.Board);

		var dummyBoardAction = new BoardAction(selectedBoardActionQValues.Board, selectedBoardActionQValues.ActionQValues.First(), board => new Board(board));
		var currentBoardAction = dummyBoardAction;
		sut.OnBoardActionQValueCalculated += boardAction => currentBoardAction = boardAction;
        //Act
        await sut.Learn();
        //Assert
        //Note: boardActionQValues gets updated during the learning.
        var expectedQValueTable = new QValueTable(boardActionQValues, learningParametersStub.Object);
        qValueWriterSpy.Verify(spy => spy.Write(expectedQValueTable));
		currentBoardAction.NextBoard.IsSolved.ShouldBeTrue();
    }

    private static void SetUpBoardFactory(Mock<BoardFactory> boardFactoryStub, Board randomBoard)
    {
        boardFactoryStub
			.Setup(stub => stub.GetRandom())
			.Returns(randomBoard);
    }

	private static void SetUpActionSelectionPolicy(Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
        int numberOfMovesTillResolved, int numberOfIterations, BoardActionQValues initialBoardActionQValues)
    {
		var boardActions = GetNextBoardActions(initialBoardActionQValues, numberOfMovesTillResolved);
		var boardActionsForEveryIteration = Enumerable.Repeat(boardActions, numberOfIterations).SelectMany(x => x);
		var queue = new Queue<BoardAction>(boardActionsForEveryIteration);
		actionSelectionPolicyStub
			.Setup(stub => stub.PickAction(It.IsAny<ActionQValues>(), It.IsAny<Board>()))
			.Returns(queue.Dequeue);
    }

    private static void SetUpQValueWriter(Mock<QValueWriter> qValueWriterSpy) =>
		qValueWriterSpy
			.Setup(dummy => dummy.Write(It.IsAny<QValueTable>()))
			.ReturnsAsync(Task.CompletedTask);

    private static void SetUpQValueReader(Mock<QValueReader> qValueReaderStub, IEnumerable<BoardActionQValues> boardActionQValues) =>
		qValueReaderStub
			.Setup(stub => stub.Read())
			.ReturnsAsync(boardActionQValues);

    private static void SetUpQLearningHyperparameters(Mock<QLearningHyperparameters> learningParametersStub) =>
		learningParametersStub
			.SetupGet(stub => stub.NumberOfIterations)
			.Returns(GetNumberOfIterations());

    /// <summary>Limit the iterations to a reasonable small number.</summary>
    private static int GetNumberOfIterations() => Random.Next(1, 10);

    private static BoardAction[] GetNextBoardActions(BoardActionQValues initialBoardActionQValues, int numberOfMovesTillResolved)
	{
		var result = new BoardAction[numberOfMovesTillResolved];
		var boardTracker = new BoardTracker();
		boardTracker.Add(initialBoardActionQValues.Board);
		var currentBoardActionQValues = initialBoardActionQValues;
		for (var i=0;i < numberOfMovesTillResolved-1;i++)
        {
            var boardAction = GetNonRepeatingBoardAction(boardTracker, currentBoardActionQValues);
            result[i] = boardAction;
            currentBoardActionQValues = GetNextBoardActionQValues(boardAction.NextBoard);
            boardTracker.Add(boardAction.NextBoard);
        }

        var lastBoardAction = new BoardActionStub(currentBoardActionQValues.Board, Board.Solved);
		result[numberOfMovesTillResolved-1] = lastBoardAction;

		return result;
	}

    private static BoardAction GetNonRepeatingBoardAction(BoardTracker boardTracker, BoardActionQValues currentBoardActionQValues)
    {
        var boardAction = GetBoardAction(currentBoardActionQValues, currentBoardActionQValues.ActionQValues);
        var remainingActions = currentBoardActionQValues.ActionQValues.Remove(boardAction.ActionQValue);

        while (boardTracker.WasProcessedBefore(boardAction.NextBoard) && remainingActions.Any())
        {
            boardAction = GetBoardAction(currentBoardActionQValues, remainingActions);
            remainingActions = remainingActions.Remove(boardAction.ActionQValue);
        }

        if (boardTracker.WasProcessedBefore(boardAction.NextBoard))
            throw new Exception("Couldn't find an action leading to a board which hasn't been processed yet.");
        return boardAction;
    }

    private static BoardAction GetBoardAction(BoardActionQValues currentBoardActionQValues, ActionQValues allPossibleActions)
    {
		var possibleActions = allPossibleActions.ToArray();
		var nextAction = possibleActions[Random.Next(0,possibleActions.Length-1)];
        return new BoardAction(currentBoardActionQValues.Board, nextAction, board => new Board(board));
    }

    private static BoardActionQValues GetNextBoardActionQValues(Board nextBoard)
	{
		var fixture = new Fixture();
		var nextMovableCells = nextBoard.GetMovableCells();
		var nextActionQValues = nextMovableCells.Select(cell => new ActionQValue(new Move(cell.NumberValue), fixture.Create<double>()));
		return new BoardActionQValues(nextBoard, new ActionQValues(nextActionQValues));
	}
}
