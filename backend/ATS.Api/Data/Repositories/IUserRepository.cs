using ATS.Api.Models;

namespace ATS.Api.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<IEnumerable<User>> GetByCompanyIdAsync(Guid companyId);
}