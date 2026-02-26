using ATS.Api.Models;

namespace ATS.Api.Data.Repositories.Interfaces;

public interface ICandidateRepository
{
    Task<Candidate> CreateAsync(Candidate candidate);
    Task<Candidate?> GetByIdAsync(Guid id);
    Task<IEnumerable<Candidate>> GetByJobIdAsync(Guid jobId);
    Task<IEnumerable<Candidate>> GetByCompanyIdAsync(Guid companyId);
    Task<Candidate> UpdateStatusAsync(Guid id, string status);
}