namespace FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;

using FifteenPuzzle.Game;

public class NonRepeatingActionSelectionPolicy
{
    private readonly IActionSelectionPolicyFactory _actionSelectionPolicyFactory;
    private readonly BoardActionFactory _boardActionFactory;
    private readonly BoardMoveTracker _boardMoveTracker;

    public NonRepeatingActionSelectionPolicy(IActionSelectionPolicyFactory actionSelectionPolicyFactory,
		BoardActionFactory boardActionFactory, BoardMoveTracker boardMoveTracker)
	{
        _actionSelectionPolicyFactory = actionSelectionPolicyFactory;
        _boardActionFactory = boardActionFactory;
        _boardMoveTracker = boardMoveTracker;
    }

    public virtual (bool, BoardAction) TryPickAction(ActionQValues actionQValues, Board currentBoard)
    {
		var remainingActions = actionQValues;
		var actionSelectionPolicy = _actionSelectionPolicyFactory.Get();
		var boardAction = PickBoardActionFromRemainingActions(actionSelectionPolicy, currentBoard, remainingActions);
		remainingActions = remainingActions.Remove(boardAction.ActionQValue);
		while (_boardMoveTracker.WasProcessedBefore(boardAction.BoardMove) && remainingActions.Any())
		{
			boardAction = PickBoardActionFromRemainingActions(actionSelectionPolicy, currentBoard, remainingActions);
			remainingActions = remainingActions.Remove(boardAction.ActionQValue);
		}

		if (!_boardMoveTracker.WasProcessedBefore(boardAction.BoardMove))
			return (true, boardAction);
		
		return (false, _boardActionFactory.GetDefault());
    }

	private BoardAction PickBoardActionFromRemainingActions(IActionSelectionPolicy actionSelectionPolicy, Board currentBoard,
		ActionQValues remainingActions)
	{
		var action = actionSelectionPolicy.PickAction(remainingActions);
		return _boardActionFactory.Get(currentBoard, action);
	}
}