using ATS.Api.Models;

namespace ATS.Api.Data.Repositories.Interfaces;

public interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<Job?> GetByIdAsync(Guid id);
    Task<IEnumerable<Job>> GetByCompanyIdAsync(Guid companyId);
    Task DeleteAsync(Guid id);
}