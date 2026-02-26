using ATS.Api.DTOs.Jobs;
using ATS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        var companyIdStr = HttpContext.Items["CompanyId"]?.ToString();
        if (string.IsNullOrEmpty(companyIdStr) || !Guid.TryParse(companyIdStr, out var companyId))
            return BadRequest(new { error = "Inget företag kopplat till användaren" });
        
        var jobs = await _jobService.GetByCompanyIdAsync(companyId);
        return Ok(jobs);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
    {
        var companyIdStr = HttpContext.Items["CompanyId"]?.ToString();
        var userIdStr = HttpContext.Items["UserId"]?.ToString();
        
        if (string.IsNullOrEmpty(companyIdStr) || !Guid.TryParse(companyIdStr, out var companyId))
            return BadRequest(new { error = "Inget företag kopplat till användaren" });
        
        var userId = Guid.Parse(userIdStr!);
        var job = await _jobService.CreateAsync(dto, companyId, userId);
        return Ok(job);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        await _jobService.DeleteAsync(id);
        return Ok();
    }
}