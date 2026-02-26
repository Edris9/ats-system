namespace ATS.Api.DTOs.Arbetsformedlingen;

public class AfImportJobDto
{
    public string AfJobId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? AfJobUrl { get; set; }
}