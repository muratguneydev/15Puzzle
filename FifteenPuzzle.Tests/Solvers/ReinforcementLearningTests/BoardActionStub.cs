namespace FifteenPuzzle.Tests.SolverTests.ReinforcementLearningTests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public record BoardActionStub : BoardAction
{
    private readonly Board _nextBoard;

    public BoardActionStub(Board board, Board nextBoard)
		: base(board, new ActionQValue(new Move(1), 0), board => new Board(board))
    {
        _nextBoard = nextBoard;
    }

    public override Board NextBoard => _nextBoard;
}