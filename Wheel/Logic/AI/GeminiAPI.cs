using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.Logic.AI
{
    internal class GeminiAPI
    {
        private const string ApiKey = "";
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=YOUR_API_KEY";

        public static async Task<string> GetGeminiResponse(string prompt)
        {
            using var client = new HttpClient();
            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            }
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiUrl.Replace("YOUR_API_KEY", ApiKey), content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }
        }
        public static string GetFullTextFromResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return string.Empty;

            try
            {
                var jsonObject = JObject.Parse(response);
                var textParts = jsonObject["candidates"]?
                    .SelectMany(candidate => candidate["content"]?["parts"] ?? Enumerable.Empty<JToken>())
                    .Select(part => part["text"]?.ToString())
                    .Where(text => !string.IsNullOrEmpty(text));

                return textParts != null ? string.Join(" ", textParts) : string.Empty;
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Invalid JSON format: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
