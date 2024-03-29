namespace FifteenPuzzle.Api.Tests;

using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;

public class GameControllerTests
{
	private static readonly GameStateDtoComparer GameStateDtoComparer = new();
	//WebApplicationFactory<TEntryPoint> is used to create a TestServer for the integration tests. TEntryPoint is the entry point class of the SUT, usually Program.cs.
	private readonly WebApplicationFactory<Program> _factory = new ();

	[Test]
	public async Task ShouldStartNewGame()
    {
        //Arrange
        var client = _factory.CreateClient();
        //Act
        var response = await client.PutAsync("/Game/new", null);
        //Assert
        response.EnsureSuccessStatusCode();

        var firstBoard = await GetBoardFromResponse(response);
        firstBoard.ShouldNotBeNull();

        //Act2
        response = await client.PutAsync("/Game/new", null);
        //Assert2
        var secondBoard = await GetBoardFromResponse(response);
        secondBoard.ShouldNotBe(firstBoard, new BoardDtoComparer());
    }

    [Test, DomainAutoData]
	public async Task ShouldGetGameState(Board board)
    {
        //Arrange
        await PutBoardInCache(board);
        var client = _factory.CreateClient();
        //Act
        var response = await client.GetAsync("/Game");
        //Assert
        response.EnsureSuccessStatusCode();

        var expected = GetExpected(board);

        var responseString = await response.Content.ReadAsStringAsync();
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(responseString)
            ?? throw new Exception("Deserialized board is null.");
		gameState.ShouldBe(expected, GameStateDtoComparer);
    }

	[Test, DomainAutoData]
	public async Task ShouldReturnNewState_WhenMoveIsMade(Board board)
    {
        //Arrange
        await PutBoardInCache(board);
        var client = _factory.CreateClient();
		var aMove = board.GetMoves().First();
        //Act
		//var numberParameter = new StringContent(aMove.Number.ToString(), Encoding.UTF8, "application/json");
        //var response = await client.PutAsync("/Game", numberParameter);
        var response = await client.PutAsync($"/Game/{aMove.Number}", null);
        //Assert
        response.EnsureSuccessStatusCode();

		board.Move(aMove.Number.ToString());
        var expected = GetExpected(board);

        var responseString = await response.Content.ReadAsStringAsync();
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(responseString)
            ?? throw new Exception("Deserialized board is null.");
		gameState.ShouldBe(expected, GameStateDtoComparer);
    }

	[Test, DomainAutoData]
	public async Task ShouldStoreNewState_WhenMoveIsMade(Board board)
    {
        //Arrange
        await PutBoardInCache(board);
        var client = _factory.CreateClient();
		var aMove = board.GetMoves().First();
        //Act
       	await client.PutAsync($"/Game/{aMove.Number}", null);
		var getCurrentBoardResponse = await client.GetAsync("/Game");
        //Assert
        getCurrentBoardResponse.EnsureSuccessStatusCode();

		board.Move(aMove.Number.ToString());
        var expected = GetExpected(board);

        var responseString = await getCurrentBoardResponse.Content.ReadAsStringAsync();
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(responseString)
            ?? throw new Exception("Deserialized board is null.");
		gameState.ShouldBe(expected, GameStateDtoComparer);
    }

	private static async Task<BoardDto> GetBoardFromResponse(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(responseString)
            ?? throw new Exception("Deserialized board is null.");
        var firstBoard = gameState.Board;
        return firstBoard;
    }

    private async Task PutBoardInCache(Board board)
    {
        var serializedBoardJson = JsonConvert.SerializeObject(board, Formatting.Indented);
        var cache = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IDistributedCache>();
        await cache.SetStringAsync(BoardSessionRepository.BoardSessionKey, serializedBoardJson);
    }

    private GameStateDto GetExpected(Board board)
    {
        var boardDto = BoardDtoProvider.Get(board);
        var expected = new GameStateDto(boardDto);
        return expected;
    }
}
