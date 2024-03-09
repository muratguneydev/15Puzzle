namespace FifteenPuzzle.Play.Cli.Tests;

using AutoFixture.NUnit3;
using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

public class GameApiClientTests//Broker?
{
	[Test, ApiAutoMoqData]
	public async Task ShouldReturnCurrentBoard(
		Board board,
		[Frozen] Mock<IHttpClientFactory> httpClientFactoryStub,
		GameApiClient sut)
	{
		//Arrange
		SetUpToReturnBoard("Game", board, httpClientFactoryStub);
		//Act
		var result = await sut.GetCurrentBoard();
		//Assert
		result.ShouldBe(board, new BoardComparer());
	}

	[Test, ApiAutoMoqData]
	public async Task ShouldReturnNewBoard(
		Board board,
		[Frozen] Mock<IHttpClientFactory> httpClientFactoryStub,
		GameApiClient sut)
    {
        //Arrange
        SetUpToReturnBoard("Game/new", board, httpClientFactoryStub);
        //Act
        var result = await sut.GetNewBoard();
        //Assert
        result.ShouldBe(board, new BoardComparer());
    }

	[Test, ApiAutoMoqData]
	public async Task ShouldReturnCorrectBoard_WhenMoveMade(
		Move move,
		Board nextBoard,
		[Frozen] Mock<IHttpClientFactory> httpClientFactoryStub,
		GameApiClient sut)
    {
        //Arrange
        SetUpToReturnBoard($"Game/{move.Number}", nextBoard, httpClientFactoryStub);
        //Act
        var result = await sut.Move(move.Number);
        //Assert
        result.ShouldBe(nextBoard, new BoardComparer());
    }

    private static void SetUpToReturnBoard(string url, Board board, Mock<IHttpClientFactory> httpClientFactoryStub)
    {
        var boardDto = BoardDtoProvider.Get(board);
        var gameStateDto = new GameStateDto(boardDto);
        var serializedGameStateJson = JsonConvert.SerializeObject(gameStateDto, Formatting.Indented);

        var httpMessageHandlerStub = new HttpMessageHandlerStub(GameApiClient.Name);
        httpMessageHandlerStub.AddResponse(url, serializedGameStateJson);
        httpMessageHandlerStub.SetUpHttpClientFactory(httpClientFactoryStub);
    }
}
