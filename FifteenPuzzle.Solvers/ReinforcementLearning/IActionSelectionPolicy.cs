namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public interface IActionSelectionPolicy
{
	ActionQValue PickAction(ActionQValues actionQValues);
}

// public class ActionSelectionPolicyFactory
// {
 
// 	public IActionSelectionPolicy Get()
// 	{

// 	}
// }

// public class NonRepeatingActionSelectionPolicy : IActionSelectionPolicy
// {
//     private readonly IActionSelectionPolicy _actionSelectionPolicy;
//     //private HashSet<Board> _boardTracker = new(new BoardComparer());

//     public NonRepeatingActionSelectionPolicy(IActionSelectionPolicy actionSelectionPolicy, HashSet<Board> boardTracker) =>
// 		_actionSelectionPolicy = actionSelectionPolicy;

//     public ActionQValue PickAction(ActionQValues boardActionQValues)
//     {
//         var result = _actionSelectionPolicy.PickAction(boardActionQValues);
//     }
// }