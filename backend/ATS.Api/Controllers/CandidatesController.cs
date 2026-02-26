using ATS.Api.DTOs.Candidates;
using ATS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidatesController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCandidates()
    {
        var companyId = Guid.Parse(HttpContext.Items["CompanyId"]!.ToString()!);
        var candidates = await _candidateService.GetByCompanyIdAsync(companyId);
        return Ok(candidates);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateDto dto)
    {
        var candidate = await _candidateService.CreateAsync(dto);
        return Ok(candidate);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateCandidateStatusDto dto)
    {
        var candidate = await _candidateService.UpdateStatusAsync(id, dto);
        return Ok(candidate);
    }
}