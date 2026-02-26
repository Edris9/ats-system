using ATS.Api.DTOs.Candidates;

namespace ATS.Api.Services.Interfaces;

public interface ICandidateService
{
    Task<CandidateResponseDto> CreateAsync(CreateCandidateDto dto);
    Task<IEnumerable<CandidateResponseDto>> GetByCompanyIdAsync(Guid companyId);
    Task<CandidateResponseDto> UpdateStatusAsync(Guid id, UpdateCandidateStatusDto dto);
}