using ATS.Api.Models;
using ATS.Api.Data.Repositories.Interfaces;
using Dapper;

namespace ATS.Api.Data.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly SupabaseClient _db;

    public CandidateRepository(SupabaseClient db)
    {
        _db = db;
    }

    public async Task<Candidate> CreateAsync(Candidate candidate)
    {
        using var conn = _db.CreateConnection();
        var sql = @"INSERT INTO candidates (id, full_name, email, phone, linkedin_url, status, notes, job_id, created_at, updated_at)
                    VALUES (@Id, @FullName, @Email, @Phone, @LinkedinUrl, @Status, @Notes, @JobId, @CreatedAt, @UpdatedAt)
                    RETURNING *";
        return await conn.QueryFirstAsync<Candidate>(sql, candidate);
    }

    public async Task<Candidate?> GetByIdAsync(Guid id)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Candidate>(
            "SELECT * FROM candidates WHERE id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Candidate>> GetByJobIdAsync(Guid jobId)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Candidate>(
            "SELECT * FROM candidates WHERE job_id = @JobId ORDER BY created_at DESC", new { JobId = jobId });
    }

    public async Task<IEnumerable<Candidate>> GetByCompanyIdAsync(Guid companyId)
    {
        using var conn = _db.CreateConnection();
        var sql = @"SELECT c.* FROM candidates c
                    INNER JOIN jobs j ON c.job_id = j.id
                    WHERE j.company_id = @CompanyId
                    ORDER BY c.created_at DESC";
        return await conn.QueryAsync<Candidate>(sql, new { CompanyId = companyId });
    }

    public async Task<Candidate> UpdateStatusAsync(Guid id, string status)
    {
        using var conn = _db.CreateConnection();
        var sql = @"UPDATE candidates SET status = @Status, updated_at = NOW() 
                    WHERE id = @Id RETURNING *";
        return await conn.QueryFirstAsync<Candidate>(sql, new { Id = id, Status = status });
    }
}