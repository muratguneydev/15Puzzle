namespace FifteenPuzzle.Api.Tests;

using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;

public class GameControllerTests
{
	//WebApplicationFactory<TEntryPoint> is used to create a TestServer for the integration tests. TEntryPoint is the entry point class of the SUT, usually Program.cs.
	private readonly WebApplicationFactory<Program> _factory = new ();

	[Test, DomainAutoData]
	public async Task ShouldGetGameState(Board board)
    {
        //Arrange
        var serializedBoardJson = JsonConvert.SerializeObject(board, Formatting.Indented);
        var cache = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IDistributedCache>();
        await cache.SetStringAsync(BoardStorage.BoardStorageKey, serializedBoardJson);

        var client = _factory.CreateClient();
        //Act
        var response = await client.GetAsync("/Game");
        //Assert
        response.EnsureSuccessStatusCode();

        var expected = GetExpected(board);

        var responseString = await response.Content.ReadAsStringAsync();
        var gameState = JsonConvert.DeserializeObject<GameStateDto>(responseString)
			?? throw new Exception("Deserialized board is null.");
        gameState.Board.ShouldBe(expected.Board, new BoardDtoComparer());
    }

    private GameStateDto GetExpected(Board board)
    {
        var boardDto = GetBoardDto(board);
        var expected = new GameStateDto(boardDto);
        return expected;
    }

    private BoardDto GetBoardDto(Board board)
    {
		var boardCells = board.Cells;
        var cellDtos = new CellDto[Board.SideLength,Board.SideLength];
		for (var row = 0; row < Board.SideLength; row++)
        {
            for (var column = 0; column < Board.SideLength; column++)
            {
                cellDtos[row, column] = new CellDto(row, column, boardCells[row,column].Value);
            }
        }
		return new (cellDtos);
    }
}
