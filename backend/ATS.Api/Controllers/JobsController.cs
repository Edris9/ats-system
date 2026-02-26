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
        var companyId = Guid.Parse(HttpContext.Items["CompanyId"]!.ToString()!);
        var jobs = await _jobService.GetByCompanyIdAsync(companyId);
        return Ok(jobs);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
    {
        var companyId = Guid.Parse(HttpContext.Items["CompanyId"]!.ToString()!);
        var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
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