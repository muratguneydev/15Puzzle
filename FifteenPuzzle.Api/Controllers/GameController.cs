namespace FifteenPuzzle.Api.Controllers;

using FifteenPuzzle.Game;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GameController(IHttpContextAccessor httpContextAccessor)
	{
        _httpContextAccessor = httpContextAccessor;
    }

	[HttpGet]
    public IActionResult GetGameState()
    {
        var gameState = _httpContextAccessor.HttpContext.Session.GetString("GameSession");
        // Process and return the game state
        return Ok(gameState);
    }

    [HttpPost]
    public IActionResult UpdateGameState([FromBody] string newGameState)
    {
        HttpContext.Session.SetString("GameSession", newGameState);
        // Process the updated game state
        return Ok();
    }

	[HttpPut]
	public async Task<IActionResult> Move(int number)
	{
		var jsonData = HttpContext.Session.GetString("Board");

		if (jsonData == null)
		{
			return NotFound("No existing game session found.");
		}

		var board = await new StringContent(jsonData).ReadFromJsonAsync<Board>();

		//await JsonContent.ReadFromJsonAsync<Board>(jsonData);

		//JsonConvert.DeserializeObject<YourComplexObject>(jsonData);


		return Ok("Game data loaded from session.");
	}

	// private static readonly string[] Summaries = new[]
	// {
	//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	// };

	// private readonly ILogger<GameController> _logger;

	// public GameController(ILogger<GameController> logger)
	// {
	//     _logger = logger;
	// }

	// [HttpGet(Name = "GetWeatherForecast")]
	// public IEnumerable<WeatherForecast> Get()
	// {
	//     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
	//     {
	//         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
	//         TemperatureC = Random.Shared.Next(-20, 55),
	//         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
	//     })
	//     .ToArray();
	// }
}
