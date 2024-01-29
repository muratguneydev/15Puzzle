namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Collections;
using FifteenPuzzle.Game;

public class ActionQValues : IEnumerable<ActionQValue>
{
    private readonly Dictionary<Move,ActionQValue> _actionQValues;

    public ActionQValues(IEnumerable<ActionQValue> actionQValues) =>
		_actionQValues = actionQValues.ToDictionary(action => action.Move);

	public double MaxQValue => _actionQValues.Any() ? _actionQValues.Values.MaxBy(a => a.QValue)!.QValue : 0;

    public ActionQValues Remove(ActionQValue actionQValue)
    {
        var clone = _actionQValues.Values.ToList();
		clone.Remove(actionQValue);
		return new ActionQValues(clone);
    }

    public ActionQValue Get(ActionQValue actionToFind)
    {
        if (_actionQValues.TryGetValue(actionToFind.Move, out var actionQValue))
			return actionQValue;
		
		throw new Exception($"ActionQValue with Move:{actionToFind.Move} not found in the collection.");
    }

    public IEnumerator<ActionQValue> GetEnumerator() => _actionQValues.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public sealed override string ToString()
		=> string.Join(',', this.Select(a => Padded(a.Move.Number) + "-" + a.QValue.ToString()));

	private string Padded(int number) => Padded(number.ToString());
	private string Padded(string value) => value.PadLeft(2, ' ');
}
