namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public record ActionQValue
{
	public ActionQValue(Move move, double qValue)
	{
        Move = move;
        QValue = qValue;
    }

    public Move Move { get; }
    public double QValue { get; private set; }

    public void UpdateQValue(double newQValue) => QValue = newQValue;
}
