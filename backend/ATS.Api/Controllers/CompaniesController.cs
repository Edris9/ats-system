using ATS.Api.Data.Repositories.Interfaces;
using ATS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public CompaniesController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        using var conn = new Npgsql.NpgsqlConnection(HttpContext.RequestServices
            .GetRequiredService<ATS.Api.Data.SupabaseClient>().CreateConnection().ConnectionString);
        await conn.OpenAsync();
        var companies = await Dapper.SqlMapper.QueryAsync<Company>(conn, "SELECT * FROM companies ORDER BY name");
        return Ok(companies);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
    {
        using var conn = new Npgsql.NpgsqlConnection(HttpContext.RequestServices
            .GetRequiredService<ATS.Api.Data.SupabaseClient>().CreateConnection().ConnectionString);
        await conn.OpenAsync();
        var company = await Dapper.SqlMapper.QueryFirstAsync<Company>(conn,
            "INSERT INTO companies (id, name, created_at, updated_at) VALUES (@Id, @Name, @CreatedAt, @UpdatedAt) RETURNING *",
            new { Id = Guid.NewGuid(), dto.Name, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        return Ok(company);
    }
}

public class CreateCompanyDto
{
    public string Name { get; set; } = string.Empty;
}