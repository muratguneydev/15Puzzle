namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;
using FifteenPuzzle.Tests.AutoFixture;
using FluentAssertions;
using global::AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Shouldly;

public class QLearningTests
{
    private static readonly Random Random = new();
	private static readonly BoardComparer BoardComparer = new();

	private static readonly Board SelectedInitialBoard = new (new[,]
        {
            {  1,  7,  9, 11 },
            {  3,  2, 12,  8 },
            {  0,  4, 13,  5 },
            { 14, 10, 15,  6 }
        });

    private static readonly Board[] OtherBoardsNotChosen = new[] {
        new Board(new[,]
        {
            { 14, 11, 13,  8 },
            {  0, 12,  9,  5 },
            { 10,  6,  3,  7 },
            { 15,  2,  4,  1 }
        }),
        new Board(new[,]
        {
            {  7, 14,  5, 12 },
            { 10,  6,  8,  0 },
            {  3,  4,  2,  1 },
            { 15, 13, 11,  9 }
        })
    };

	private static readonly Board[] BoardsInSelectedPath = new[] {
		SelectedInitialBoard,//initial board
		new Board(new[,]//next move, 3 moves down
        {
            {  1,  7,  9, 11 },
            {  0,  2, 12,  8 },
            {  3,  4, 13,  5 },
            { 14, 10, 15,  6 }
        }),
        new Board(new[,]//next move, 1 moves down
        {
            {  0,  7,  9, 11 },
            {  1,  2, 12,  8 },
            {  3,  4, 13,  5 },
            { 14, 10, 15,  6 }
        }),
		Board.Solved
	};

	private static readonly BoardActionQValues InitialSelectedBoardActionQValues =
		new (SelectedInitialBoard, GetActionWithRandomQValues(SelectedInitialBoard));

	private static readonly BoardActionQValues[] OtherNotSelectedBoardActionQValues = OtherBoardsNotChosen
		.Select(board => new BoardActionQValues(board, GetActionWithRandomQValues(board)))
		.ToArray();

    private static readonly BoardActionQValues[] InitialBoardActionQValues = OtherNotSelectedBoardActionQValues
		.Append(InitialSelectedBoardActionQValues)
		.ToArray();

	private static readonly BoardAction[] InitialBoardActionsInSelectedPath = 
		new[] {
			new BoardAction(BoardsInSelectedPath[0], new ActionQValue(new Move(3), InitialSelectedBoardActionQValues.ActionQValues.Single(aqv => aqv.Move.Number == 3).QValue), Clone),
			new BoardAction(BoardsInSelectedPath[1], new ActionQValue(new Move(1), QValueTable.DefaultQValue), Clone),
			new BoardActionStub(BoardsInSelectedPath[2], Board.Solved)//leading to a mocked solved state
		};

    [Test, AutoMoqData]
    public async Task ShouldStopAfterNIterations(
		[Frozen][Mock] Mock<QLearningHyperparameters> learningParametersStub,
		[Frozen][Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen][Mock] Mock<QValueWriter> qValueWriterDummy,
		[Frozen][Mock] Mock<BoardFactory> boardFactoryStub,
		[Frozen][Mock] Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
		QLearning sut)
    {
        //Arrange
        SetUpQLearningHyperparameters(learningParametersStub);
        SetUpQValueReader(qValueReaderStub);
        SetUpQValueWriter(qValueWriterDummy);

        var numberOfIterations = learningParametersStub.Object.NumberOfIterations;
        SetUpActionSelectionPolicy(actionSelectionPolicyStub, numberOfIterations);
        SetUpBoardFactory(boardFactoryStub);

        var iterationCounter = 0;
        sut.OnIterationCompleted += numberOfIterations => iterationCounter = numberOfIterations;
        //Act
        await sut.Learn();
        //Assert
        iterationCounter.Should().Be(numberOfIterations);
    }

    [Test, AutoMoqData]
    public async Task Should_LoadExistingQValuesAtStart_AndPersistAtTheEnd(
		double reward,
		double calculatedQValue,
        [Frozen][Mock] Mock<QLearningHyperparameters> learningParametersStub,
        [Frozen][Mock] Mock<QValueReader> qValueReaderStub,
        [Frozen][Mock] Mock<QValueWriter> qValueWriterSpy,
        [Frozen][Mock] Mock<BoardFactory> boardFactoryStub,
        [Frozen][Mock] Mock<QValueCalculator> qValueCalculatorStub,
        [Frozen][Mock] Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
		[Frozen] Mock<IRewardStrategy> rewardStrategyStub,
        QLearning sut)
    {
        //Arrange
        SetUpQLearningHyperparameters(learningParametersStub);
        SetUpQValueReader(qValueReaderStub);
        SetUpQValueWriter(qValueWriterSpy);
        SetUpRewardStrategy(rewardStrategyStub, reward);

        var numberOfIterations = learningParametersStub.Object.NumberOfIterations;
        SetUpActionSelectionPolicy(actionSelectionPolicyStub, numberOfIterations);
        SetUpBoardFactory(boardFactoryStub);

        var boardActionComparer = new BoardActionComparer();
        qValueCalculatorStub
            .Setup(stub => stub.Calculate(It.Is<BoardAction>(ba => InitialBoardActionsInSelectedPath.Contains(ba, boardActionComparer)),
                It.IsAny<QValueTable>(),//TODO: At least verify that the boards in the table are the expected boards.
                reward))
            .Returns((BoardAction _, QValueTable _, double _) =>
            {
                return calculatedQValue;//Note: Always returning the same calculated value. Vary for each board?
            });

        //Act
        await sut.Learn();
        //Assert
        var expectedQValueTable = GetExpectedQValueTable(calculatedQValue);
        qValueWriterSpy.Verify(spy => spy.Write(It.Is<QValueTable>(qvt => VerifyQValueTable(qvt, expectedQValueTable))));
    }

    [Test, AutoMoqData]
    public async Task Should_UpdateQValuesUntilBoardIsResolved(
        [Frozen][Mock] Mock<QLearningHyperparameters> learningParametersStub,
        [Frozen][Mock] Mock<BoardFactory> boardFactoryStub,
        [Frozen][Mock] Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
        QLearning sut)
    {
        //Arrange
        SetUpQLearningHyperparameters(learningParametersStub);

        var numberOfIterations = learningParametersStub.Object.NumberOfIterations;
        SetUpActionSelectionPolicy(actionSelectionPolicyStub, numberOfIterations);
        SetUpBoardFactory(boardFactoryStub);

        var dummyBoardAction = new BoardAction(SelectedInitialBoard, InitialSelectedBoardActionQValues.ActionQValues.First(), board => new Board(board));
        var currentBoardAction = dummyBoardAction;
        sut.OnBoardActionQValueCalculated += boardAction => currentBoardAction = boardAction;
        //Act
        await sut.Learn();
        //Assert
        currentBoardAction.NextBoard.IsSolved.ShouldBeTrue();
    }

    private static void SetUpBoardFactory(Mock<BoardFactory> boardFactoryStub) =>
		boardFactoryStub
            .Setup(stub => stub.GetSolvable())
            .Returns(SelectedInitialBoard);

    private static void SetUpActionSelectionPolicy(Mock<NonRepeatingActionSelectionPolicy> actionSelectionPolicyStub,
        int numberOfIterations)
    {
		var boardActions = InitialBoardActionsInSelectedPath;
        var boardActionsForEveryIteration = Enumerable.Repeat(boardActions, numberOfIterations).SelectMany(x => x);
        var queue = new Queue<BoardAction>(boardActionsForEveryIteration);
        actionSelectionPolicyStub
            .Setup(stub => stub.TryPickAction(It.IsAny<ActionQValues>(), It.IsAny<Board>()))
            .Returns((ActionQValues _, Board _) => (true, queue.Dequeue()));
    }

    private static void SetUpQValueWriter(Mock<QValueWriter> qValueWriterSpy) =>
        qValueWriterSpy
            .Setup(dummy => dummy.Write(It.IsAny<QValueTable>()))
            .ReturnsAsync(Task.CompletedTask);

    private static void SetUpQValueReader(Mock<QValueReader> qValueReaderStub) =>
        qValueReaderStub
            .Setup(stub => stub.Read())
            .ReturnsAsync(InitialBoardActionQValues);

	private static void SetUpRewardStrategy(Mock<IRewardStrategy> rewardStrategyStub, double reward) =>
		rewardStrategyStub
			.Setup(stub => stub.Calculate(It.Is<Board>(board => BoardsInSelectedPath.Contains(board, BoardComparer))))
			.Returns(reward);//TODO: different reward for different boards?

    private static void SetUpQLearningHyperparameters(Mock<QLearningHyperparameters> learningParametersStub) =>
        learningParametersStub
            .SetupGet(stub => stub.NumberOfIterations)
            .Returns(GetNumberOfIterations());

    /// <summary>Limit the iterations to a reasonable small number.</summary>
    private static int GetNumberOfIterations() => Random.Next(1, 10);

    private static ActionQValues GetActionWithRandomQValues(Board board) =>
		new (board.GetMoves()
				  .Select(move => new ActionQValue(move, Random.NextDouble())));

    private static ActionQValues GetActionQValues(Board boardInSelectedPath, double qValue)
    {
        var boardAction = InitialBoardActionsInSelectedPath.Single(ba => BoardComparer.Equals(ba.Board, boardInSelectedPath));
		return new(boardInSelectedPath.GetMoves()
                  .Select(move => new ActionQValue(move, move.Equals(boardAction.ActionQValue.Move) ? qValue : 0)));
    }

    private static Func<Board,Board> Clone => board => new Board(board);

	private static bool VerifyQValueTable(QValueTable actual, QValueTable expectedQValueTable)
    {
        actual.ShouldBe(expectedQValueTable, new QValueTableComparer());
		return true;
    }

    private static QValueTable GetExpectedQValueTable(double calculatedQValue)
    {
        var calculatedBoardActionQValues = GetCalculatedBoardActionQValues(calculatedQValue);
        return new QValueTable(calculatedBoardActionQValues
								//Note:Below union excludes the initial selected board as it is already in the above list.
                                .UnionBy(InitialBoardActionQValues, b => b.Board, BoardComparer));
    }

    private static IEnumerable<BoardActionQValues> GetCalculatedBoardActionQValues(double calculatedQValue) =>
		InitialBoardActionQValues
			.Select(baqv => GetBoardActionQValues(baqv, calculatedQValue))
			.Union(InitialBoardActionsInSelectedPath.Select(ba => new BoardActionQValues(ba.Board, GetActionQValues(ba.Board, calculatedQValue))));

    private static BoardActionQValues GetBoardActionQValues(BoardActionQValues baqv, double calculatedQValue) =>
		new (baqv.Board, new ActionQValues(
				baqv
				.ActionQValues
				.Select(aqv => new ActionQValue(aqv.Move, InitialBoardActionsInSelectedPath
					.Any(ba => ba.ActionQValue.Move.Equals(aqv.Move) && InitialBoardActionsInSelectedPath.Any(ba => BoardComparer.Equals(ba.Board, baqv.Board)))
								? calculatedQValue
								: aqv.QValue))));
}
