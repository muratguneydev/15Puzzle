namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardActionFactory
{
    private readonly BoardFactory _boardFactory;

    public BoardActionFactory(BoardFactory boardFactory) => _boardFactory = boardFactory;
	
    public virtual BoardAction Get(Board board, ActionQValue action) => new (board, action, _boardFactory.Clone);
}