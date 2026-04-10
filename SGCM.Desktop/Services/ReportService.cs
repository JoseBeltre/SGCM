using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class ReportService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/reports";

        public ReportService()
        {
            _client = ApiClient.Client;
        }

        public async Task<AppointmentStatsDto> GetAppointmentStatsAsync()
        {
            // Ruta asumida: api/reports/appointments-stats
            var response = await _client.GetAsync($"{Endpoint}/appointments-stats");
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<AppointmentStatsDto>(ApiClient.JsonOptions) ?? new AppointmentStatsDto();
        }
    }
}
