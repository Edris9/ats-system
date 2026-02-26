using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ATS.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace ATS.Api.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var secret = _configuration["Jwt:Secret"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("companyId", user.CompanyId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}