using System.Text;
using System.Text.Json;

namespace AITaskTracker.API.Services;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> SummarizeAsync(string text)
    {
        var apiKey = _configuration["GeminiSettings:ApiKey"];
        var model = _configuration["GeminiSettings:Model"] ?? "gemini-2.0-flash";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API key is missing.");
        }

        var endpoint =
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $"Summarize the following learning log in 1 short sentence:\n\n{text}"
                        }
                    }
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(requestBody);

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        var responseJson = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Gemini Status Code: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine($"Gemini Response Body: {responseJson}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Gemini API request failed. StatusCode: {(int)response.StatusCode} {response.StatusCode}. Body: {responseJson}"
            );
        }

        using var document = JsonDocument.Parse(responseJson);

        var candidates = document.RootElement.GetProperty("candidates");

        if (candidates.GetArrayLength() == 0)
        {
            return "Summary could not be generated.";
        }

        var content = candidates[0].GetProperty("content");
        var parts = content.GetProperty("parts");

        if (parts.GetArrayLength() == 0)
        {
            return "Summary could not be generated.";
        }

        var summary = parts[0].GetProperty("text").GetString();

        return summary ?? "Summary could not be generated.";
    }
}