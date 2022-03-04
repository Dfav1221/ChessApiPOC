using ChessEngineInterfacePOC.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChessEngineInterfacePOC.Controllers;

[ApiController]
[Route("")]
public class EngineController : Controller
{
    private readonly IEngineService _engineService;

    public EngineController(IEngineService engineService)
    {
        _engineService = engineService;
    }

    [HttpGet("test")]
    public Task<IActionResult> CalculatePosition([FromRoute] int time,string fenPosition)
    {
        var response = _engineService.CalculatePosition(fenPosition, time, 'W');
        return Task.FromResult<IActionResult>(Ok(response));
    }
}