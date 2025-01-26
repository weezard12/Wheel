using Newtonsoft.Json;
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


        private static async Task<string> GetGeminiResponse(string prompt)
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
    }
}
