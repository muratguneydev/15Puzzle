namespace FifteenPuzzle.Tests.CLI;

using AutoFixture.NUnit3;
using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.CLI;
using FifteenPuzzle.Game;
using FifteenPuzzle.Tests.Common;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

public class ApiClientTests//Broker?
{
	[Test, ApiAutoMoqData]
	public async Task ShouldReturnCurrentBoard(
		Board board,
		[Frozen] Mock<IHttpClientFactory> httpClientFactoryStub,
		ApiClient sut)
	{
		//Arrange
		var bardDto = BoardDtoProvider.Get(board);
		var gameStateDto = new GameStateDto(bardDto);
		var serializedGameStateJson = JsonConvert.SerializeObject(gameStateDto, Formatting.Indented);

		var httpMessageHandlerStub = new HttpMessageHandlerStub(ApiClient.Name);
		httpMessageHandlerStub.AddResponse("Game", serializedGameStateJson);
		httpMessageHandlerStub.SetUpHttpClientFactory(httpClientFactoryStub);
		//Act
		var result = await sut.GetCurrentBoard();
		//Assert
		result.ShouldBe(board, new BoardComparer());
	}
}
