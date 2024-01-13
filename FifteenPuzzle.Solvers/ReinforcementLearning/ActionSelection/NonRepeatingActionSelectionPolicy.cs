namespace FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;

using FifteenPuzzle.Game;

public class NonRepeatingActionSelectionPolicy
{
    private readonly IActionSelectionPolicyFactory _actionSelectionPolicyFactory;
    private readonly BoardActionFactory _boardActionFactory;

    public NonRepeatingActionSelectionPolicy(IActionSelectionPolicyFactory actionSelectionPolicyFactory,
		BoardActionFactory boardActionFactory)
	{
        _actionSelectionPolicyFactory = actionSelectionPolicyFactory;
        _boardActionFactory = boardActionFactory;
    }

    public virtual BoardAction PickAction(ActionQValues actionQValues, Board currentBoard, HashSet<Board> boardTracker)
    {
		var remainingActions = actionQValues;
		var actionSelectionPolicy = _actionSelectionPolicyFactory.Get();
		var boardAction = PickBoardActionFromRemainingActions(actionSelectionPolicy, currentBoard, remainingActions);
		remainingActions = remainingActions.Remove(boardAction.ActionQValue);
		while (boardTracker.Contains(boardAction.NextBoard) && remainingActions.Any())
		{
			boardAction = PickBoardActionFromRemainingActions(actionSelectionPolicy, currentBoard, remainingActions);
			remainingActions = remainingActions.Remove(boardAction.ActionQValue);
		}

		if (!boardTracker.Contains(boardAction.NextBoard))
			return boardAction;
		throw new Exception("Couldn't find an action leading to a board which hasn't been processed yet.");//TODO:create a test
    }

	private BoardAction PickBoardActionFromRemainingActions(IActionSelectionPolicy actionSelectionPolicy, Board currentBoard,
		ActionQValues remainingActions)
	{
		var action = actionSelectionPolicy.PickAction(remainingActions);
		return _boardActionFactory.Get(currentBoard, action);
	}
}