namespace FifteenPuzzle.Solvers.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using FifteenPuzzle.Solvers.Contracts;

[ApiController]
[Route("[controller]")]
public class ActionQualityController : ControllerBase
{
    private readonly QualityValueRepository _qualityValueRepository;

    public ActionQualityController(QualityValueRepository qualityValueRepository)
	{
        _qualityValueRepository = qualityValueRepository;
    }

	[HttpGet("{boardHashCode}")]
    public async Task<IActionResult> Get(int boardHashCode, CancellationToken cancellationToken)
    {
		//TODO: Do this separately rather than every time
		await _qualityValueRepository.Refresh(cancellationToken);

        var actionQValues = await _qualityValueRepository.Get(boardHashCode, cancellationToken);
		var dtos = actionQValues.Select(aqv => new ActionQualityValueDto(new MoveDto(aqv.Move.Number), aqv.QValue));
		
        return Ok(dtos);
    }
}


