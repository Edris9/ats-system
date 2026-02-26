using ATS.Api.DTOs.Auth;

namespace ATS.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<LoginResponseDto> CreateUserAsync(CreateUserDto dto);
}