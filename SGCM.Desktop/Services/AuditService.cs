using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class AuditService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/audits";

        public AuditService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<AuditLogDto>> GetAuditLogsAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<AuditLogDto>>(ApiClient.JsonOptions) ?? new List<AuditLogDto>();
        }
    }
}
