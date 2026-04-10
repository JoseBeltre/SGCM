using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class AvailabilityService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/availability";

        public AvailabilityService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<AvailabilityDto>> GetAvailabilitiesAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<AvailabilityDto>>(ApiClient.JsonOptions) ?? new List<AvailabilityDto>();
        }

        public async Task<AvailabilityDto> CreateAsync(AvailabilityCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync(Endpoint, dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<AvailabilityDto>(ApiClient.JsonOptions);
        }

        public async Task UpdateAsync(int id, AvailabilityUpdateDto dto)
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
