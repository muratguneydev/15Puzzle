using System.Collections;

namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public class ActionQValues : IEnumerable<ActionQValue>
{
    private readonly IEnumerable<ActionQValue> _actionQValues;

    public ActionQValues(IEnumerable<ActionQValue> actionQValues) => _actionQValues = actionQValues.ToArray();

    public IEnumerator<ActionQValue> GetEnumerator() => _actionQValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
