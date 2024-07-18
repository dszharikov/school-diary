using System.Text.Json;
using Homework.DTOs;

namespace Homework.Services;

public class TermService : ITermService
{
    private readonly HttpClient _httpClient;

    public TermService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TermDto?> GetTermAsync(int termId)
    {
        var uri = $"{termId}";

        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TermDto>(content);
    }
}