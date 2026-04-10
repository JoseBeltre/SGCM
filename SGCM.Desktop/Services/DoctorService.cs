using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class DoctorService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/doctors";

        public DoctorService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<DoctorDto>> GetDoctorsAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<DoctorDto>>(ApiClient.JsonOptions) ?? new List<DoctorDto>();
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync($"{Endpoint}/{id}");
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<DoctorDto>(ApiClient.JsonOptions);
        }

        public async Task<DoctorDto> CreateAsync(AddDoctorDto dto)
        {
            var response = await _client.PostAsJsonAsync(Endpoint, dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<DoctorDto>(ApiClient.JsonOptions);
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto dto)
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
