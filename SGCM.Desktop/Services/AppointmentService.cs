using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class AppointmentService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/appointments";

        public AppointmentService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>(ApiClient.JsonOptions) ?? new List<AppointmentDto>();
        }

        public async Task<List<AppointmentDto>> GetByDoctorAsync(int doctorId)
        {
            // Ruta asumida para filtrado por doctor: api/appointments/doctor/{id}
            var response = await _client.GetAsync($"{Endpoint}/doctor/{doctorId}");
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>(ApiClient.JsonOptions) ?? new List<AppointmentDto>();
        }

        public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync(Endpoint, dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<AppointmentDto>(ApiClient.JsonOptions);
        }

        public async Task UpdateAsync(int id, AppointmentUpdateDto dto)
        {
            var response = await _client.PutAsJsonAsync($"{Endpoint}/{id}", dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"{Endpoint}/{id}");
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }

        public async Task ConfirmAsync(int id)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Endpoint}/{id}/confirm");
            var response = await _client.SendAsync(request);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }

        public async Task CompleteAsync(int id)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Endpoint}/{id}/complete");
            var response = await _client.SendAsync(request);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }

        public async Task CancelAsync(int id, string reason)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Endpoint}/{id}/cancel");
            request.Content = JsonContent.Create(new { CancellationReason = reason }, null, ApiClient.JsonOptions);
            var response = await _client.SendAsync(request);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }
    }
}
