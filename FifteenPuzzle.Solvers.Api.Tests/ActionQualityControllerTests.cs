namespace FifteenPuzzle.Solvers.Api.Tests;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.Contracts;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;

public class ActionQualityControllerTests
{
	private static readonly BoardComparer BoardComparer = new();

    [Test, DomainAutoData]
	public async Task ShouldGetActionQualityValues(BoardActionQValues[] boardActionQValuesCollection)
    {
        //Arrange
		var factory = GetWebApplicationFactory(boardActionQValuesCollection);
        var client = factory.CreateClient();
		var expected = boardActionQValuesCollection[0];
        //Act
		var key = GetKey(expected);
        var response = await client.GetAsync($"/ActionQuality/{key}");
        //Assert
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var actionQualityValues = JsonConvert.DeserializeObject<IEnumerable<ActionQualityValueDto>>(responseString)
            ?? throw new Exception("Deserialized actional quality result is null.");
		
		var expectedDtos = expected.ActionQValues.Select(aqv => new ActionQualityValueDto(new MoveDto(aqv.Move.Number), aqv.QValue));
		actionQualityValues.ShouldBe(expectedDtos, ignoreOrder: true);
    }

	private WebApplicationFactory<Program> GetWebApplicationFactory(BoardActionQValues[] boardActionQValuesCollection)
	{
		var fileContent = new BoardActionQValuesStringConverter().GetBoardQValueFileContent(boardActionQValuesCollection);
		var fileSystemFake = new FileSystemFake(fileContent);

		var factory = new CustomWebApplicationFactory(services =>
		{
			services.AddTransient<FileSystem>(_ => fileSystemFake);
		});
		//https://dotnetfromthemountain.com/aspnet-core-testing-strategies-the-basics/
		//https://khalidabuhakmeh.com/testing-aspnet-core-6-apps

		return factory;
	}

    // private async Task PutBoardActionQValuesInCache(IEnumerable<BoardActionQValues> boardActionQValuesCollection)
    // {
	// 	var cache = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IDistributedCache>();

	// 	foreach (var boardActionQValues in boardActionQValuesCollection)
	// 	{
	// 		var serializedJson = JsonConvert.SerializeObject(boardActionQValues.ActionQValues, new MoveConverter());
    //     	await cache.SetStringAsync(GetKey(boardActionQValues).ToString(), serializedJson);
	// 	}
    // }

	private static int GetKey(BoardActionQValues boardActionQValue) =>
			BoardComparer.GetHashCode(boardActionQValue.Board);

	
}
