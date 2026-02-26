using ATS.Api.Data.Repositories.Interfaces;
using ATS.Api.DTOs.Auth;
using ATS.Api.Exceptions;
using ATS.Api.Helpers;
using ATS.Api.Models;
using ATS.Api.Services.Interfaces;

namespace ATS.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedException("Fel email eller lösenord");

        return new LoginResponseDto
        {
            Token = _jwtHelper.GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CompanyId = user.CompanyId ?? Guid.Empty
        };
    }

    public async Task<LoginResponseDto> CreateUserAsync(CreateUserDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new AppException("Email används redan");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FullName = dto.FullName,
            Role = dto.Role,
            CompanyId = dto.CompanyId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.CreateAsync(user);

        return new LoginResponseDto
        {
            Token = _jwtHelper.GenerateToken(created),
            FullName = created.FullName,
            Email = created.Email,
            Role = created.Role,
            CompanyId = created.CompanyId ?? Guid.Empty
        };
    }
}