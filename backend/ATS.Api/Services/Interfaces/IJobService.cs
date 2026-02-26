using ATS.Api.DTOs.Jobs;

namespace ATS.Api.Services.Interfaces;

public interface IJobService
{
    Task<JobResponseDto> CreateAsync(CreateJobDto dto, Guid companyId, Guid userId);
    Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(Guid companyId);
    Task DeleteAsync(Guid id);
}