namespace FifteenPuzzle.Api.Controllers;

using System;
using FifteenPuzzle.Api.Contracts;
using FifteenPuzzle.Game;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly BoardStorage _boardStorage;

    public GameController(BoardStorage boardStorage)
	{
        _boardStorage = boardStorage;
    }

	[HttpGet]
    public async Task<IActionResult> GetGameState(CancellationToken cancellationToken)
    {
        var board = await _boardStorage.Get(cancellationToken);
		var boardDto = GetBoardDto(board);
		var gameState = new GameStateDto(boardDto);
        return Ok(gameState);
    }

    private BoardDto GetBoardDto(Board board)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult UpdateGameState([FromBody] string newGameState)
    {
        
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
