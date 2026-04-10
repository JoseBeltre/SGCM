using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SGCM.Desktop.Models;

namespace SGCM.Desktop.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private const string Endpoint = "api/users";

        public UserService()
        {
            _client = ApiClient.Client;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _client.GetAsync(Endpoint);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<List<UserDto>>(ApiClient.JsonOptions) ?? new List<UserDto>();
        }

        public async Task<UserDto> CreateAsync(UserCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync(Endpoint, dto, ApiClient.JsonOptions);
            await ApiClient.EnsureSuccessOrThrowAsync(response);
            return await response.Content.ReadFromJsonAsync<UserDto>(ApiClient.JsonOptions);
        }

        public async Task UpdateAsync(int id, UserUpdateDto dto)
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
