using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class SpecialtyService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/specialties";

        public SpecialtyService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<SpecialtyDto>> GetSpecialtiesAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<SpecialtyDto>>(ApiClient.JsonOptions) ?? new List<SpecialtyDto>();
        }

        public async Task<SpecialtyDto> CreateAsync(SpecialtyCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync(Endpoint, dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<SpecialtyDto>(ApiClient.JsonOptions);
        }

        public async Task UpdateAsync(int id, SpecialtyUpdateDto dto)
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
