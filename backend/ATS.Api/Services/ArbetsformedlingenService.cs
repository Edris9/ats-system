using ATS.Api.DTOs.Arbetsformedlingen;
using ATS.Api.Services.Interfaces;
using System.Text.Json;

namespace ATS.Api.Services;

public class ArbetsformedlingenService : IArbetsformedlingenService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ArbetsformedlingenService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["Arbetsformedlingen:BaseUrl"]!;
    }

    public async Task<IEnumerable<AfJobSearchResultDto>> SearchJobsAsync(string query)
    {
        var url = $"{_baseUrl}/search?q={Uri.EscapeDataString(query)}&limit=10";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        var hits = document.RootElement.GetProperty("hits");

        var results = new List<AfJobSearchResultDto>();
        foreach (var hit in hits.EnumerateArray())
        {
            results.Add(new AfJobSearchResultDto
            {
                Id = hit.GetProperty("id").GetString() ?? "",
                Headline = hit.GetProperty("headline").GetString() ?? "",
                Description = hit.TryGetProperty("description", out var desc)
                    ? desc.TryGetProperty("text", out var text) ? text.GetString() : null
                    : null,
                WorkplaceName = hit.TryGetProperty("employer", out var employer)
                    ? employer.TryGetProperty("name", out var name) ? name.GetString() : null
                    : null,
                Municipality = hit.TryGetProperty("workplace_address", out var address)
                    ? address.TryGetProperty("municipality", out var municipality) ? municipality.GetString() : null
                    : null,
                Url = hit.TryGetProperty("webpage_url", out var url2) ? url2.GetString() : null
            });
        }

        return results;
    }
}