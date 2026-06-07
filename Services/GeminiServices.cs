using System.Text;
using System.Text.Json;

namespace ToyotaWeb.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"];
        }

    public async Task<string> Ask(string message)
{
    try
    {
      var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key={_apiKey}";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = message }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine(result); // 🔥 debug

        var doc = JsonDocument.Parse(result);

        if (doc.RootElement.TryGetProperty("error", out var error))
        {
            return "Lỗi AI: " + error.GetProperty("message").GetString();
        }

        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "Không có phản hồi";
    }
    catch (Exception ex)
    {
        return "Lỗi AI: " + ex.Message;
    }
}
    }
}