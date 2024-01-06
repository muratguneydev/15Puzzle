namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using System.Collections;

public class ActionQValues : IEnumerable<ActionQValue>
{
    private readonly IEnumerable<ActionQValue> _actionQValues;

    public ActionQValues(IEnumerable<ActionQValue> actionQValues) => _actionQValues = actionQValues.ToArray();

	public double MaxQValue => _actionQValues.Any() ? _actionQValues.MaxBy(a => a.QValue)!.QValue : 0;

	public static ActionQValues Empty => new ActionQValues(Enumerable.Empty<ActionQValue>());

    public IEnumerator<ActionQValue> GetEnumerator() => _actionQValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
