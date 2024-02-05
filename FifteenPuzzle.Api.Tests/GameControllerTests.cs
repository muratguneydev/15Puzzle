using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FifteenPuzzle.Api.Tests;

// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.TestHost;

public class GameControllerTests
{
	//WebApplicationFactory<TEntryPoint> is used to create a TestServer for the integration tests. TEntryPoint is the entry point class of the SUT, usually Program.cs.
	private readonly WebApplicationFactory<Program> _factory = new ();

	[Test, DomainAutoData]
	public async Task Test1(Board board)
	{
		//Arrange
		var serializedBoardJson = JsonConvert.SerializeObject(board, Formatting.Indented);
		var cache = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IDistributedCache>();
		await cache.SetStringAsync(BoardStorage.BoardStorageKey, serializedBoardJson);

        var client = _factory.CreateClient();
		//Act
		var response = await client.GetAsync("/Game");
		//Assert
		response.EnsureSuccessStatusCode(); // Status Code 200-299
        // Assert.Equal("text/html; charset=utf-8", 
        //     response.Content.Headers.ContentType.ToString());
        var responseString = await response.Content.ReadAsStringAsync();
        
	}
}
