namespace FifteenPuzzle.Solvers.ReinforcementLearning.ActionSelection;

using FifteenPuzzle.Game;

public class NonRepeatingActionSelectionPolicy
{
    private readonly IActionSelectionPolicyFactory _actionSelectionPolicyFactory;
    private readonly BoardActionFactory _boardActionFactory;
    private readonly BoardTracker _boardTracker;

    public NonRepeatingActionSelectionPolicy(IActionSelectionPolicyFactory actionSelectionPolicyFactory,
		BoardActionFactory boardActionFactory, BoardTracker boardTracker)
	{
        _actionSelectionPolicyFactory = actionSelectionPolicyFactory;
        _boardActionFactory = boardActionFactory;
        _boardTracker = boardTracker;
    }

    public virtual (bool, BoardAction) TryPickAction(ActionQValues actionQValues, Board currentBoard)
    {
		var remainingActions = actionQValues;
		var actionSelectionPolicy = _actionSelectionPolicyFactory.Get();
		var boardAction = PickBoardActionFromRemainingActions(actionSelectionPolicy, currentBoard, remainingActions);
		remainingActions = remainingActions.Remove(boardAction.ActionQValue);
		while (_boardTracker.WasProcessedBefore(boardAction.NextBoard) && remainingActions.Any())
		{
			boardAction = PickBoardActionFromRemainingActions(actionSelectionPolicy, currentBoard, remainingActions);
			remainingActions = remainingActions.Remove(boardAction.ActionQValue);
		}

		if (!_boardTracker.WasProcessedBefore(boardAction.NextBoard))
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