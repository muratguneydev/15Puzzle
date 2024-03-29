namespace FifteenPuzzle.Api.Tests;

using System.Text;
using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

public class BoardSessionRepositoryTests
{
	private static readonly DistributedCacheEntryOptionsComparer CacheEntryOptionsComparer = new();

	[Test, AutoMoqData]
	public async Task ShoulGetBoard(Board expectedBoard,
		CancellationToken cancellationToken,
		[Frozen] Mock<IDistributedCache> cacheStub,
		BoardSessionRepository sut)
	{
		//Arrange
		var serializedBoardJson = JsonConvert.SerializeObject(expectedBoard, Formatting.Indented);
		cacheStub
			.Setup(stub => stub.GetAsync(BoardSessionRepository.BoardSessionKey, cancellationToken))
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
		BoardSessionRepository sut)
	{
		//Act
		await sut.Update(expectedBoard, cancellationToken);
		//Assert
		var serializedBoardJson = JsonConvert.SerializeObject(expectedBoard, Formatting.Indented);
		var serializedBoardJsonBytes = Encoding.UTF8.GetBytes(serializedBoardJson);
		cacheSpy.Verify(spy => spy.SetAsync(BoardSessionRepository.BoardSessionKey, serializedBoardJsonBytes,
			It.Is<DistributedCacheEntryOptions>(o => CacheEntryOptionsComparer.Equals(o, new DistributedCacheEntryOptions())),
			cancellationToken));
	}
}
