using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class PatientService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/patients";

        public PatientService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<PatientDto>> GetPatientsAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<PatientDto>>(ApiClient.JsonOptions) ?? new List<PatientDto>();
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync($"{Endpoint}/{id}");
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<PatientDto>(ApiClient.JsonOptions);
        }

        public async Task<PatientDto> CreateAsync(AddPatientDto dto)
        {
            var response = await _client.PostAsJsonAsync(Endpoint, dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<PatientDto>(ApiClient.JsonOptions);
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            var response = await _client.PutAsJsonAsync($"{Endpoint}/{id}", dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"{Endpoint}/{id}");
            await ApiClient.EnsureSuccessOrThrowAsync(response);
        }
    }
}
