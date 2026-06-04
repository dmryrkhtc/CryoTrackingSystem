using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CryoTracking.Application.Interfaces;

namespace CryoTracking.Infrastructure.Repositories
{
    public class EmbryoAIService : IEmbryoAIService
    {
        private readonly HttpClient _httpClient;
        // Canlıya aldığın Hugging Face API adresin
        private const string AiServiceUrl = "https://htcdmryrk-embryo-quality-service.hf.space/predict";

        public EmbryoAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double?> GetAIScoreAsync(IFormFile embryoImage)
        {
            try
            {
                if (embryoImage == null || embryoImage.Length == 0)
                    return null;

                using var content = new MultipartFormDataContent();
                using var ms = new MemoryStream();
                await embryoImage.CopyToAsync(ms);

                var byteContent = new ByteArrayContent(ms.ToArray());
                // Python FastAPI tarafındaki parametre ismi "file"
                content.Add(byteContent, "file", embryoImage.FileName);

                var response = await _httpClient.PostAsync(AiServiceUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AIResponseDto>();
                    if (result != null && result.Success)
                    {
                        return result.AiScore;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AI Service Error]: {ex.Message}");
                return null;
            }
        }
    }

    // İçeride kullanacağımız küçük response modeli
    public class AIResponseDto
    {
        public bool Success { get; set; }
        public double AiScore { get; set; }
        public string Message { get; set; }
    }
}