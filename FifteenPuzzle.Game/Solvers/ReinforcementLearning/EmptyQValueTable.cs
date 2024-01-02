namespace FifteenPuzzle.Game.Solvers.ReinforcementLearning;

public record EmptyQValueTable() : QValueTable(Enumerable.Empty<BoardActionQValues>());
