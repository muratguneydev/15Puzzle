namespace FifteenPuzzle.Game;

public record RandomBoard : Board
{
    public RandomBoard()
        : base(Solved) => RandomizeWithFisherYatesShuffle();

    private void RandomizeWithFisherYatesShuffle()
    {
        var rand = new Random();

        int n = Cells.GetLength(0) * Cells.GetLength(1);
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            Swap(i, j);
        }
    }

    private void Swap(int i, int j)
    {
        var temp = Cells[i / 4, i % 4];
        Cells[i / 4, i % 4] = Cells[j / 4, j % 4];
        Cells[j / 4, j % 4] = temp;
    }
}