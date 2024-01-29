namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public record BoardActionQValues(Board Board, ActionQValues ActionQValues)
{
    public sealed override string ToString() => $"{Board} - {ActionQValues}";
}
