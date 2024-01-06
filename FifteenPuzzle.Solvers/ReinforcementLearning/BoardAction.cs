namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class BoardAction
{
	public BoardAction(Board board, ActionQValue actionQValue, Func<Board,Board> cloneFactory)
	{
        Board = board;
        ActionQValue = actionQValue;
		
		NextBoard = cloneFactory(board);
		NextBoard.Move(actionQValue.Move.Number.ToString());
    }

    public Board Board { get; }
    public Board NextBoard { get; }
    public ActionQValue ActionQValue { get; }
}