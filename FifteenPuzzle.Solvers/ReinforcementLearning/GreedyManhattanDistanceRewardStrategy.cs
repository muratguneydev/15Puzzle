namespace FifteenPuzzle.Solvers.ReinforcementLearning;

using FifteenPuzzle.Game;

public class GreedyManhattanDistanceRewardStrategy : IRewardStrategy
{
    public double Calculate(Board board)
    {
        var size = Board.SideLength;
    	var reward = 0.0;
		var cells = board.Cells;
		var solvedBoard = Board.Solved;

		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				var number = cells[i, j].Value;
				// Skip the empty tile ???
				if (string.IsNullOrEmpty(number))
				{
					continue;
				}
				var solvedCell = solvedBoard.GetCell(number);
				// Calculate the Manhattan distance and add it to the reward
				int manhattanDistance = Math.Abs(i - solvedCell.Row) + Math.Abs(j - solvedCell.Column);
				reward -= manhattanDistance; // Negative because we want to minimize the distance
			}
		}

		return reward;
    }
}
