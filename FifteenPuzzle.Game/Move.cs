namespace FifteenPuzzle.Game;

public record Move
{
    public Move(int number)
    {
		var maxNumber = Board.SideLength * Board.SideLength - 1;
		if (number < 1 || number > maxNumber)
		{
			throw new ArgumentException($"The number to move should be between 1 and {maxNumber}");
		}

		Number = number;
    }

	public Move(string number)
		: this(int.Parse(number))
	{
		
	}

	public int Number { get; }
}
