namespace FifteenPuzzle.Solvers.Api.Tests;

using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System.Text;

public class QualityValueRepositoryTests
{
	private static readonly DistributedCacheEntryOptionsComparer CacheEntryOptionsComparer = new();
	private static readonly BoardComparer BoardComparer = new();

	[Test, AutoMoqData]
	public async Task Should_ReturnEmptyActionQValues_WhenBoardNotFound(int key,
		CancellationToken cancellationToken,
		[Frozen] Mock<IDistributedCache> cacheStub,
		QualityValueRepository sut)
	{
		//Arrange
		cacheStub
			.Setup(stub => stub.GetAsync(key.ToString(), cancellationToken))
			.ReturnsAsync(null as byte[]);
		//Act
		var result = await sut.Get(key, cancellationToken);
		//Assert
		result.ShouldBeEmpty();
	}
	
	[Test, AutoMoqData]
	public async Task ShoulGetActionQValues(BoardActionQValues boardActionQValues,
		CancellationToken cancellationToken,
		[Frozen] Mock<IDistributedCache> cacheStub,
		QualityValueRepository sut)
	{
		//Arrange
		var serializedJson = JsonConvert.SerializeObject(boardActionQValues.ActionQValues, new MoveConverter());
		var key = GetKey(boardActionQValues);
		cacheStub
			.Setup(stub => stub.GetAsync(key.ToString(), cancellationToken))
			.ReturnsAsync(Encoding.UTF8.GetBytes(serializedJson));
		//Act
		var result = await sut.Get(key, cancellationToken);
		//Assert
		result.ShouldBe(boardActionQValues.ActionQValues, new ActionQValueComparer());
	}

	[Test, AutoMoqData]
	public async Task ShoulRefreshBoardActionQValues(IEnumerable<BoardActionQValues> boardActionQValuesCollection,
		CancellationToken cancellationToken,
		[Frozen] [Mock] Mock<QValueReader> qValueReaderStub,
		[Frozen] Mock<IDistributedCache> cacheSpy,
		QualityValueRepository sut)
	{
		//Arrange
		qValueReaderStub
			.Setup(stub => stub.Read())
			.ReturnsAsync(boardActionQValuesCollection);
		//Act
		await sut.Refresh(cancellationToken);
		//Assert
		foreach (var boardActionQValues in boardActionQValuesCollection)
		{
			var serializedJson = JsonConvert.SerializeObject(boardActionQValues.ActionQValues, Formatting.Indented);
			var serializedJsonBytes = Encoding.UTF8.GetBytes(serializedJson);
			cacheSpy.Verify(spy => spy.SetAsync(GetKey(boardActionQValues).ToString(), serializedJsonBytes,
				It.Is<DistributedCacheEntryOptions>(o => CacheEntryOptionsComparer.Equals(o, new DistributedCacheEntryOptions())),
				cancellationToken));
		}
	}

	private static int GetKey(BoardActionQValues boardActionQValue) =>
		BoardComparer.GetHashCode(boardActionQValue.Board);
}
