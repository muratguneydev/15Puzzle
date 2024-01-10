namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Collections;

public class ActionQValues : IEnumerable<ActionQValue>
{
    private readonly List<ActionQValue> _actionQValues;

    public ActionQValues(IEnumerable<ActionQValue> actionQValues) => _actionQValues = actionQValues.ToList();

	public double MaxQValue => _actionQValues.Any() ? _actionQValues.MaxBy(a => a.QValue)!.QValue : 0;
	//public int Count => _actionQValues.Count;

    public ActionQValues Remove(ActionQValue actionQValue)
    {
        var clone = _actionQValues.ToList();
		clone.Remove(actionQValue);
		return new ActionQValues(clone);
    }

    public IEnumerator<ActionQValue> GetEnumerator() => _actionQValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
