using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcommercePlatform.ChatAssistant;

/// <summary>
/// Very small wrapper around the Ollama local LLM HTTP API (https://ollama.ai/).
/// </summary>
public class OllamaLLMClient
{
    private readonly HttpClient _httpClient;
    private readonly string _modelName;

    public OllamaLLMClient(string baseUrl, string modelName)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
        _modelName = modelName;
    }

    /// <summary>
    /// Sends the <paramref name="prompt"/> to Ollama and returns the concatenated response text.
    /// </summary>
    public async Task<string> GenerateResponseAsync(string prompt)
    {
        var payload = new
        {
            model = _modelName,
            prompt
        };

        var requestContent = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("/api/generate", requestContent);
        response.EnsureSuccessStatusCode();

        var raw = await response.Content.ReadAsStringAsync();

        // Ollama streams JSON objects separated by newlines. We need to concatenate the "response" fields.
        var jsonLines = raw.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
        var builder = new StringBuilder();
        foreach (var line in jsonLines)
        {
            using var doc = JsonDocument.Parse(line);
            if (doc.RootElement.TryGetProperty("response", out var respProp))
            {
                builder.Append(respProp.GetString());
            }
        }

        var fullResponse = builder.ToString();
        // Remove Ollama's "<think>...</think>" blocks if present.
        fullResponse = Regex.Replace(fullResponse, "<think>.*?</think>", string.Empty, RegexOptions.Singleline).Trim();
        return fullResponse;
    }
} 