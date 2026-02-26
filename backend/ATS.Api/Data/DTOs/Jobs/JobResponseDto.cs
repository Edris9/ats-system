namespace ATS.Api.DTOs.Jobs;

public class JobResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? AfJobId { get; set; }
    public string? AfJobUrl { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
}