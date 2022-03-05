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
    public async Task<IActionResult> CalculatePosition(int time,string fenPosition)
    {
        var response =await _engineService.CalculatePosition(fenPosition, time, "W");
        return Ok(response);
    }
}