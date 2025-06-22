using System.Text.Json;
using System.Text;

namespace SupportSystemApp.Application.Services
{
    public class GeminiAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:GeminiApiKey"];
        }

        public async Task<string> GetAIResponseAsync(string userPrompt)
        {
            string prompt = "Sana uygulama içinde, son kullanıcıdan soru sorulacaktır. Sohbet şeklinde değil de, tek seferlik olarak yanıt ver, prompt :";
            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt + userPrompt }
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API error {response.StatusCode}: {responseBody}");
            }

            using var doc = JsonDocument.Parse(responseBody);
            var text = doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString();

            return text ?? "AI yanıtı alınamadı.";
        }

        public async Task<string> GetPriortyAsync(string title, string description)
        {
            string prompt = " Şu anda senden istediğim, sana vereceğimtitle ve description'ını (ticket bilgisi), \"Acil\", \"Orta\" ya da \"Düşük\" öncelikli olarak sınıflandırman." +
                " Sadece bu üç stringten birini uygun görüp çıktı olarak vermeni istiyorum: \n";
            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt + "Title: " + title + "\n Description: " + description}
                        }
                    } 
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API error {response.StatusCode}: {responseBody}");
            }

            using var doc = JsonDocument.Parse(responseBody);
            var text = doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString();

            return text ?? "AI yanıtı alınamadı.";
        }
    }
}
