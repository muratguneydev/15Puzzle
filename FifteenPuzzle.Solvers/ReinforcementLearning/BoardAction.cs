namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public record BoardAction
{
    private readonly Board _nextBoard;

	public BoardAction(Board board, ActionQValue actionQValue, Func<Board,Board> cloneFactory)
	{
        Board = board;
        ActionQValue = actionQValue;
		
		_nextBoard = cloneFactory(board);
		_nextBoard.Move(actionQValue.Move.Number.ToString());
    }

    public Board Board { get; }
    public virtual Board NextBoard => _nextBoard;
    public ActionQValue ActionQValue { get; }

	public BoardMove BoardMove => new(Board, ActionQValue.Move);
}
