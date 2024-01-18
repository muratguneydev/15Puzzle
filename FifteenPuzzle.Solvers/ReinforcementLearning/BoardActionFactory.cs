namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardActionFactory
{
    private readonly BoardFactory _boardFactory;

    public BoardActionFactory(BoardFactory boardFactory) => _boardFactory = boardFactory;
	
    public virtual BoardAction Get(Board board, ActionQValue action) => new (board, action, _boardFactory.Clone);
    public virtual BoardAction GetDefault()
    {
		var board = Board.Solved;
		var action = new ActionQValue(board.GetMoves().First(), default);
        return new(board, action, _boardFactory.Clone);
    }
}