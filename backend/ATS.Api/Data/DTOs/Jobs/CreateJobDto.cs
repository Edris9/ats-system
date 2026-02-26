namespace ATS.Api.DTOs.Jobs;

public class CreateJobDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? AfJobId { get; set; }
    public string? AfJobUrl { get; set; }
}