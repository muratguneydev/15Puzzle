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
        var action = actionSelectionPolicy.PickAction(actionQValues);
		var boardAction = _boardActionFactory.Get(currentBoard, action);
		while (boardTracker.Contains(boardAction.NextBoard) && remainingActions.Any())
		{
			remainingActions = remainingActions.Remove(action);
			action = actionSelectionPolicy.PickAction(remainingActions);
			boardAction = _boardActionFactory.Get(currentBoard, action);;
		}

		if (!boardTracker.Contains(boardAction.NextBoard))
			return boardAction;
		throw new Exception("Couldn't find an action leading to a board which hasn't been processed yet.");
    }
}