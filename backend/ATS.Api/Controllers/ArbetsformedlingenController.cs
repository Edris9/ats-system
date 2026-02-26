using ATS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArbetsformedlingenController : ControllerBase
{
    private readonly IArbetsformedlingenService _afService;

    public ArbetsformedlingenController(IArbetsformedlingenService afService)
    {
        _afService = afService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var results = await _afService.SearchJobsAsync(q);
        return Ok(results);
    }
}