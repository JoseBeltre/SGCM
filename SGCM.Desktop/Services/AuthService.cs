using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/auth/login";

        public AuthService()
        {
            // El auth service usa el cliente base pero NO necesita token para loguearse
            _client = ApiClient.Client;
        }

        /// <summary>
        /// Autentica al usuario contra la API y retorna el LoginResponse completo.
        /// Lanza HttpRequestException si las credenciales son inválidas.
        /// </summary>
        public async Task<LoginResponse?> LoginAsync(string email, string password)
        {
            var request = new LoginRequest { Email = email, Password = password };
            var response = await _client.PostAsJsonAsync(Endpoint, request, ApiClient.JsonOptions);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(ApiClient.JsonOptions);
                return result;
            }

            // Intentar leer el mensaje de error del body
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Login fallido ({(int)response.StatusCode}): {error}");
        }
    }
}
