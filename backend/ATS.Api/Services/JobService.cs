using ATS.Api.Data.Repositories.Interfaces;
using ATS.Api.DTOs.Jobs;
using ATS.Api.Exceptions;
using ATS.Api.Models;
using ATS.Api.Services.Interfaces;

namespace ATS.Api.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobResponseDto> CreateAsync(CreateJobDto dto, Guid companyId, Guid userId)
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            AfJobId = dto.AfJobId,
            AfJobUrl = dto.AfJobUrl,
            CompanyId = companyId,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _jobRepository.CreateAsync(job);
        return MapToDto(created);
    }

    public async Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(Guid companyId)
    {
        var jobs = await _jobRepository.GetByCompanyIdAsync(companyId);
        return jobs.Select(MapToDto);
    }

    public async Task DeleteAsync(Guid id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        if (job == null) throw new NotFoundException("Jobbet hittades inte");
        await _jobRepository.DeleteAsync(id);
    }

    private JobResponseDto MapToDto(Job job) => new()
    {
        Id = job.Id,
        Title = job.Title,
        Description = job.Description,
        Location = job.Location,
        AfJobId = job.AfJobId,
        AfJobUrl = job.AfJobUrl,
        CompanyId = job.CompanyId,
        CreatedAt = job.CreatedAt
    };
}