using System.Collections;

namespace FifteenPuzzle.Game;

public record Row : IEnumerable<int>
{
    private readonly List<int> _items;

    public Row(int[] items) => _items = new(items);

    public IEnumerator<int> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
} 