using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public static class ApiClient
    {
        private static readonly HttpClient _httpClient;
        
        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        static ApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("https://localhost:7241/") };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClient Client
        {
            get
            {
                // Inyectar el token si existe en la sesión
                if (Session.IsLoggedIn)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", Session.Token);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
                return _httpClient;
            }
        }

        public static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API Error {response.StatusCode}: {content}");
            }
        }
    }
}
