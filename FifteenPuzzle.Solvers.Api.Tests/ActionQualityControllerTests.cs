namespace FifteenPuzzle.Solvers.Api.Tests;

using System.Text;
using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.Contracts;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using FifteenPuzzle.Tests.Common;
using FifteenPuzzle.Tests.Common.AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;

public class ActionQualityControllerTests
{
	//WebApplicationFactory<TEntryPoint> is used to create a TestServer for the integration tests. TEntryPoint is the entry point class of the SUT, usually Program.cs.
	private readonly WebApplicationFactory<Program> _factory = new ();

    [Test, DomainAutoData]
	public async Task ShouldGetActionQualityValues(Board board)
    {
        //Arrange
        //await PutBoardInCache(board);
        var client = _factory.CreateClient();
        //Act
		var boardHashCode = 0;
        var response = await client.GetAsync($"/ActionQuality/{boardHashCode}");
        //Assert
        response.EnsureSuccessStatusCode();

        //var expected = BoardDtoProvider.Get(board);

        var responseString = await response.Content.ReadAsStringAsync();
        var actionQualityValues = JsonConvert.DeserializeObject<IEnumerable<ActionQualityValueDto>>(responseString)
            ?? throw new Exception("Deserialized actional quality result is null.");
		//actionQualityValues.ShouldBe(expected, GameStateDtoComparer);
		actionQualityValues.ShouldBeEmpty();
    }
}

