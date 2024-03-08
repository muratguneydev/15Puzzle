namespace FifteenPuzzle.Play.Cli.Tests;

using AutoFixture.NUnit3;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.Contracts;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common;
using Moq;
using Newtonsoft.Json;
using Shouldly;

public class SolversApiClientTests
{
	private static readonly BoardComparer BoardComparer = new();

	[Test, ApiAutoMoqData]
	public async Task ShouldReturnActionQualityValues(
		BoardActionQValues boardActionQValues,
		[Frozen] Mock<IHttpClientFactory> httpClientFactoryStub,
		SolversApiClient sut)
	{
		//Arrange
		var key = BoardComparer.GetHashCode(boardActionQValues.Board);
		var url = $"ActionQuality/{key}";
		SetUpToReturnActionQualityValues(url, boardActionQValues.ActionQValues, httpClientFactoryStub);
		//Act
		var result = await sut.GetActionQualityValues(boardActionQValues.Board);
		//Assert
		result.ShouldBe(boardActionQValues.ActionQValues, new ActionQValuesComparer());
	}

	private static void SetUpToReturnActionQualityValues(string url, ActionQValues actionQValues, Mock<IHttpClientFactory> httpClientFactoryStub)
    {
        var expectedDtos = GetDtos(actionQValues);
        var serializedJson = JsonConvert.SerializeObject(expectedDtos, Formatting.Indented);

        var httpMessageHandlerStub = new HttpMessageHandlerStub(SolversApiClient.Name);
        httpMessageHandlerStub.AddResponse(url, serializedJson);
        httpMessageHandlerStub.SetUpHttpClientFactory(httpClientFactoryStub);
    }

    private static IEnumerable<ActionQualityValueDto> GetDtos(ActionQValues actionQValues)
    {
        return actionQValues.Select(aqv => new ActionQualityValueDto(new MoveDto(aqv.Move.Number), aqv.QValue));
    }
}