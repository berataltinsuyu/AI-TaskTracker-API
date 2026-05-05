using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using AITaskTracker.API.DTOs;

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

    public async Task<GenerateQuizResponseDto> GenerateQuizAsync(string topic)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            throw new ArgumentException("Topic cannot be empty.");
        }

        var apiKey = _configuration["GeminiSettings:ApiKey"];
        var model = _configuration["GeminiSettings:Model"] ?? "gemma-3-1b-it";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Gemini API key is missing.");
        }

        var endpoint =
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

        var prompt =
            "Create a 5-question multiple choice quiz about the following topic: " + topic + "\n\n" +
            "Return only valid JSON. Do not include markdown. Do not include explanations.\n\n" +
            "JSON format:\n" +
            "{\n" +
            "  \"topic\": \"" + topic + "\",\n" +
            "  \"questions\": [\n" +
            "    {\n" +
            "      \"question\": \"Question text\",\n" +
            "      \"options\": [\"Option A\", \"Option B\", \"Option C\", \"Option D\"],\n" +
            "      \"answer\": \"Correct option text\"\n" +
            "    }\n" +
            "  ]\n" +
            "}";

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
                                text = prompt
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

            Console.WriteLine($"Gemini Quiz Status Code: {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine($"Gemini Quiz Response Body: {responseJson}");

            if (!response.IsSuccessStatusCode)
            {
               throw new Exception(
                  $"Gemini API quiz request failed. StatusCode:{(int)response.StatusCode} {response.StatusCode}. Body:{responseJson}"
               );
            }

            var aiText = ExtractGeminiText(responseJson);
            
            var cleanedJson = CleanJsonResponse(aiText);
            
            var quiz = JsonSerializer.Deserialize<GenerateQuizResponseDto>(
              cleanedJson,
              new JsonSerializerOptions
              {
                PropertyNameCaseInsensitive = true
              }
            );

            if (quiz is null)
            {
                throw new Exception("Quiz response could not be parsed.");
            }

            return quiz;
        }

        private static string ExtractGeminiText(string responseJson)
        {
            using var document = JsonDocument.Parse(responseJson);

            if (!document.RootElement.TryGetProperty("candidates", out var candidates))
            {
                throw new Exception("Gemini response does not contain candidates.");
            }

            if (candidates.GetArrayLength() == 0)
            {
                throw new Exception("Gemini response candidates list is empty.");
            }

            var firstCandidate = candidates[0];

            if (!firstCandidate.TryGetProperty("content", out var content))
            {
                throw new Exception("Gemini response candidate does not contain content.");
            }

            if (!content.TryGetProperty("parts", out var parts))
            {
                throw new Exception("Gemini response content does not contain parts.");
            }

            if (parts.GetArrayLength() == 0)
            {
                throw new Exception("Gemini response parts list is empty.");
            }

            var firstPart = parts[0];

            if (!firstPart.TryGetProperty("text", out var textElement))
            {
                throw new Exception("Gemini response part does not contain text.");
            }

            var text = textElement.GetString();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception("Gemini response text is empty.");
            }

            return text;
        }

        private static string CleanJsonResponse(string text)
        {
          var cleaned = text.Trim();

          if (cleaned.StartsWith("```json"))
          {
            cleaned = cleaned.Replace("```json", string.Empty);
          }
          if (cleaned.StartsWith("```"))
          {
            cleaned = cleaned.Replace("```", string.Empty);
          }

          if (cleaned.EndsWith("```"))
          {
            cleaned = cleaned[..^3];
          }

          return cleaned.Trim();
        }
    }