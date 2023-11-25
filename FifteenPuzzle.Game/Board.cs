namespace FifteenPuzzle.Game;

public class Board
{
	private int[,] _cells = new int[4,4];


	public Board(int[,] cells)
	{
		if (cells.GetUpperBound(0) != 3)
		{
			throw new Exception("Board should have 3 rows.");
		}

		if (cells.GetUpperBound(1) != 3)
		{
			throw new Exception("Board should have 3 columns.");
		}

		Cells = cells;
	}

	public int[,] Cells { get => _cells; set => _cells = value; }
}