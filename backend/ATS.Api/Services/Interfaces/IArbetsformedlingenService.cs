using ATS.Api.DTOs.Arbetsformedlingen;

namespace ATS.Api.Services.Interfaces;

public interface IArbetsformedlingenService
{
    Task<IEnumerable<AfJobSearchResultDto>> SearchJobsAsync(string query);
}