namespace ATS.Api.DTOs.Auth;

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "customer";
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
}