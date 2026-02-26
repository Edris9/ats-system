using ATS.Api.Data.Repositories.Interfaces;
using ATS.Api.DTOs.Candidates;
using ATS.Api.Exceptions;
using ATS.Api.Models;
using ATS.Api.Services.Interfaces;

namespace ATS.Api.Services;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _candidateRepository;

    public CandidateService(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<CandidateResponseDto> CreateAsync(CreateCandidateDto dto)
    {
        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            LinkedinUrl = dto.LinkedinUrl,
            Status = "new",
            Notes = dto.Notes,
            JobId = dto.JobId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _candidateRepository.CreateAsync(candidate);
        return MapToDto(created);
    }

    public async Task<IEnumerable<CandidateResponseDto>> GetByCompanyIdAsync(Guid companyId)
    {
        var candidates = await _candidateRepository.GetByCompanyIdAsync(companyId);
        return candidates.Select(MapToDto);
    }

    public async Task<CandidateResponseDto> UpdateStatusAsync(Guid id, UpdateCandidateStatusDto dto)
    {
        var candidate = await _candidateRepository.GetByIdAsync(id);
        if (candidate == null) throw new NotFoundException("Kandidaten hittades inte");

        var updated = await _candidateRepository.UpdateStatusAsync(id, dto.Status);
        return MapToDto(updated);
    }

    private CandidateResponseDto MapToDto(Candidate candidate) => new()
    {
        Id = candidate.Id,
        FullName = candidate.FullName,
        Email = candidate.Email,
        Phone = candidate.Phone,
        LinkedinUrl = candidate.LinkedinUrl,
        Status = candidate.Status,
        Notes = candidate.Notes,
        JobId = candidate.JobId,
        CreatedAt = candidate.CreatedAt
    };
}