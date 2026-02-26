namespace ATS.Api.DTOs.Arbetsformedlingen;

public class AfJobSearchResultDto
{
    public string Id { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? WorkplaceName { get; set; }
    public string? Municipality { get; set; }
    public string? Url { get; set; }
}