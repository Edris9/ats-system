using ATS.Api.Models;
using ATS.Api.Data.Repositories.Interfaces;
using Npgsql;
using Dapper;

namespace ATS.Api.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SupabaseClient _db;

    public UserRepository(SupabaseClient db)
    {
        _db = db;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE email = @Email", new { Email = email });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE id = @Id", new { Id = id });
    }

    public async Task<User> CreateAsync(User user)
    {
        using var conn = _db.CreateConnection();
        var sql = @"INSERT INTO users (id, email, password_hash, full_name, role, company_id, created_at, updated_at)
                    VALUES (@Id, @Email, @PasswordHash, @FullName, @Role, @CompanyId, @CreatedAt, @UpdatedAt)
                    RETURNING *";
        return await conn.QueryFirstAsync<User>(sql, user);
    }

    public async Task<IEnumerable<User>> GetByCompanyIdAsync(Guid companyId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<User>(
            "SELECT * FROM users WHERE company_id = @CompanyId", new { CompanyId = companyId });
    }
}