using ATS.Api.Models;
using ATS.Api.Data.Repositories.Interfaces;
using Dapper;

namespace ATS.Api.Data.Repositories;

public class JobRepository : IJobRepository
{
    private readonly SupabaseClient _db;

    public JobRepository(SupabaseClient db)
    {
        _db = db;
    }

    public async Task<Job> CreateAsync(Job job)
    {
        using var conn = _db.CreateConnection();
        var sql = @"INSERT INTO jobs (id, title, description, location, af_job_id, af_job_url, company_id, created_by, created_at, updated_at)
                    VALUES (@Id, @Title, @Description, @Location, @AfJobId, @AfJobUrl, @CompanyId, @CreatedBy, @CreatedAt, @UpdatedAt)
                    RETURNING *";
        return await conn.QueryFirstAsync<Job>(sql, job);
    }

    public async Task<Job?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Job>(
            "SELECT * FROM jobs WHERE id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Job>> GetByCompanyIdAsync(Guid companyId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Job>(
            "SELECT * FROM jobs WHERE company_id = @CompanyId ORDER BY created_at DESC", new { CompanyId = companyId });
    }

    public async Task DeleteAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM jobs WHERE id = @Id", new { Id = id });
    }
}