namespace FifteenPuzzle.Game;

public record RandomBoard : Board
{
	public RandomBoard()
		: base(RandomizeWithFisherYatesShuffle(Solved))
	{

	}

	private static string[,] RandomizeWithFisherYatesShuffle(Board board)
	{
		var rand = new Random();
		var stringArray = ConvertCellsToStringArray(board.Cells);

		int n = stringArray.GetLength(0) * stringArray.GetLength(1);
		for (int i = n - 1; i > 0; i--)
		{
			int j = rand.Next(0, i + 1);
			Swap(stringArray, i, j);
		}

		return stringArray;
	}

	private static void Swap(string[,] stringArray, int i, int j)
	{
		var firstX = i / 4;
		var firstY = i % 4;
		var secondX = j / 4;
		var secondY = j % 4;
		var temp = stringArray[firstX, firstY];
		stringArray[firstX, firstY] = stringArray[secondX, secondY];
		stringArray[secondX, secondY] = temp;
	}

	static string[,] ConvertCellsToStringArray(Cell[,] cells)
	{
		int rows = cells.GetLength(0);
		int cols = cells.GetLength(1);

		string[,] stringArray = new string[rows, cols];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				stringArray[i, j] = cells[i, j].Value;
			}
		}

		return stringArray;
	}
}