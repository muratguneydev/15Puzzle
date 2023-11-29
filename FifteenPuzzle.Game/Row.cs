namespace FifteenPuzzle.Game;

using System.Collections;

public record Row : IEnumerable<Cell>
{
    private readonly IList<Cell> _items;

    public Row(Cell[] cells) => _items = new List<Cell>(cells);

    public IEnumerator<Cell> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
} 