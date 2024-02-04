namespace FifteenPuzzle.Tests.API;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Distributed;

public class DistributedCacheEntryOptionsComparer : IEqualityComparer<DistributedCacheEntryOptions>
{
	private const int ForceEquals = default;

    public bool Equals(DistributedCacheEntryOptions? x, DistributedCacheEntryOptions? y) =>
		x != null && y != null && AreEqual(x, y);

    public int GetHashCode([DisallowNull] DistributedCacheEntryOptions obj) => ForceEquals;

    private bool AreEqual(DistributedCacheEntryOptions x, DistributedCacheEntryOptions y) =>
		x.AbsoluteExpiration == y.AbsoluteExpiration
        && x.AbsoluteExpirationRelativeToNow == y.AbsoluteExpirationRelativeToNow
        && x.SlidingExpiration == y.SlidingExpiration;
}