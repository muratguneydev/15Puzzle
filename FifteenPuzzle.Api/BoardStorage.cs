namespace FifteenPuzzle.Api;

using System.Text;
using FifteenPuzzle.Game;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

public class BoardStorage
{
    public const string BoardStorageKey = "Board";
    private readonly IDistributedCache _distributedCache;

    public BoardStorage(IDistributedCache distributedCache) => _distributedCache = distributedCache;

    public virtual async Task<Board> Get(CancellationToken cancellationToken)
	{
		var serializedBoardJsonBytes = await _distributedCache.GetAsync(BoardStorageKey, cancellationToken)
		 	?? throw new Exception("Couldn't find any board in the cache.");
		var serializedBoardJson = Encoding.UTF8.GetString(serializedBoardJsonBytes);

		var cells = JsonConvert.DeserializeObject<Cell[,]>(serializedBoardJson)
			?? throw new Exception("Couldn't deserialize cells.");
		var board = new Board(cells);
		return board;
	}

	// public async Task Update(Board board, CancellationToken cancellationToken)
	// {

	// }
}
