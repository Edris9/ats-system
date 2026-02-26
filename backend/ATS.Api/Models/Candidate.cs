namespace ATS.Api.Models;

public class Candidate
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedinUrl { get; set; }
    public string Status { get; set; } = "new";
    public string? Notes { get; set; }
    public Guid JobId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}