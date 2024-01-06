namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public interface IActionSelectionPolicy
{
	ActionQValue PickAction(BoardActionQValues boardActionQValues);
}