namespace FifteenPuzzle.Solvers.Api;

using System.Text;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

public class QualityValueRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly QValueReader _qValueReader;
	private static readonly BoardComparer BoardComparer = new();

    public QualityValueRepository(IDistributedCache distributedCache, QValueReader qValueReader)
    {
        _distributedCache = distributedCache;
        _qValueReader = qValueReader;
    }

    public virtual async Task<IEnumerable<ActionQValue>> Get(int boardHashCode, CancellationToken cancellationToken)
	{
		var serializedActionQValuesJsonBytes = await _distributedCache.GetAsync(boardHashCode.ToString(), cancellationToken)
		 	?? throw new Exception("Couldn't find any board in the cache.");
		var serializedActionQValuesJson = Encoding.UTF8.GetString(serializedActionQValuesJsonBytes);
		var actionQValues = JsonConvert.DeserializeObject<IEnumerable<ActionQValue>>(serializedActionQValuesJson, new MoveConverter())
			?? throw new Exception("Couldn't deserialize cells.");
		return actionQValues;
	}

	public async Task Refresh(CancellationToken cancellationToken)
    {
        var boardActionQValues = await _qValueReader.Read();
		//TODO:clear cache
        await PopulateCache(boardActionQValues, cancellationToken);
    }

    private async Task PopulateCache(IEnumerable<BoardActionQValues> boardActionQValues, CancellationToken cancellationToken)
    {
        foreach (var boardActionQValue in boardActionQValues)
        {
            await SaveItemToCache(boardActionQValue, cancellationToken);
        }
    }

    private async Task SaveItemToCache(BoardActionQValues boardActionQValue, CancellationToken cancellationToken)
    {
        var serializedJson = JsonConvert.SerializeObject(boardActionQValue.ActionQValues, new MoveConverter());
        var serializedJsonBytes = Encoding.UTF8.GetBytes(serializedJson);
        await _distributedCache.SetAsync(GetKey(boardActionQValue), serializedJsonBytes, new DistributedCacheEntryOptions(), cancellationToken);
    }

    private static string GetKey(BoardActionQValues boardActionQValue) =>
		BoardComparer.GetHashCode(boardActionQValue.Board).ToString();
}
