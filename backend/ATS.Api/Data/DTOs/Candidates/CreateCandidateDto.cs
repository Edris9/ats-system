namespace ATS.Api.DTOs.Candidates;

public class CreateCandidateDto
{
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedinUrl { get; set; }
    public string? Notes { get; set; }
    public Guid JobId { get; set; }
}