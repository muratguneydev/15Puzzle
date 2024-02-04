namespace FifteenPuzzle.Tests.API;

using System.Text;
using FifteenPuzzle.Api;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.AutoFixture;
using global::AutoFixture.NUnit3;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

public class BoardStorageTests
{
	private static readonly DistributedCacheEntryOptionsComparer CacheEntryOptionsComparer = new();

	[Test, AutoMoqData]
	public async Task ShoulGetBoard(Board expectedBoard,
		CancellationToken cancellationToken,
		[Frozen] Mock<IDistributedCache> cacheStub,
		BoardStorage sut)
	{
		//Arrange
		var serializedBoardJson = JsonConvert.SerializeObject(expectedBoard, Formatting.Indented);
		cacheStub
			.Setup(stub => stub.GetAsync(BoardStorage.BoardStorageKey, cancellationToken))
			.ReturnsAsync(Encoding.UTF8.GetBytes(serializedBoardJson));
		//Act
		var retrievedBoard = await sut.Get(cancellationToken);
		//Assert
		retrievedBoard.ShouldBe(expectedBoard, new BoardComparer());
	}

	[Test, AutoMoqData]
	public async Task ShoulUpdateBoard(Board expectedBoard,
		CancellationToken cancellationToken,
		[Frozen] Mock<IDistributedCache> cacheSpy,
		BoardStorage sut)
	{
		//Act
		await sut.Update(expectedBoard, cancellationToken);
		//Assert
		var serializedBoardJson = JsonConvert.SerializeObject(expectedBoard, Formatting.Indented);
		var serializedBoardJsonBytes = Encoding.UTF8.GetBytes(serializedBoardJson);
		cacheSpy.Verify(spy => spy.SetAsync(BoardStorage.BoardStorageKey, serializedBoardJsonBytes,
			It.Is<DistributedCacheEntryOptions>(o => CacheEntryOptionsComparer.Equals(o, new DistributedCacheEntryOptions())),
			cancellationToken));
	}
}
