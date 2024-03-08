namespace FifteenPuzzle.Play.Cli;

using FifteenPuzzle.Game;
using FifteenPuzzle.Solvers.Contracts;
using FifteenPuzzle.Solvers.ReinforcementLearning;
using Newtonsoft.Json;

public class SolversApiClient
{
	public const string Name = "SolversApiHttpClient";
	private static readonly BoardComparer BoardComparer = new();
    private readonly IHttpClientFactory _httpClientFactory;

    public SolversApiClient(IHttpClientFactory httpClientFactory)
	{
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ActionQValues> GetActionQualityValues(Board board)
    {
        var httpClient = _httpClientFactory.CreateClient(Name);

		var key = BoardComparer.GetHashCode(board);
		var url = $"/ActionQuality/{key}";

		var response = await httpClient.GetStringAsync(url);
        var actionQualityValueDtos = JsonConvert.DeserializeObject<IEnumerable<ActionQualityValueDto>>(response)
            ?? throw new Exception("Deserialized action quality value collection is null.");
		var actionQValues = new ActionQValues(actionQualityValueDtos.Select(dto => new ActionQValue(new Move(dto.Move.Number), dto.QualityValue)));
		return actionQValues;
    }

	
}