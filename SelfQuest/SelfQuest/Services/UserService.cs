// SelfQuest/Services/UserService.cs
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KindQuestAPI.Models;
using SelfQuest.Models;

namespace SelfQuest.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Users"; // Case-sensitive!

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET /api/Users
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>(_baseUrl);
        }

        // GET /api/Users/{id}
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"{_baseUrl}/{id}");
        }

        // POST /api/Users
        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, userDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        // PUT /api/Users/{id}
        public async Task<UserDto> UpdateUserAsync(string id, UserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", userDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        // DELETE /api/Users/{id}
        public async Task<bool> DeleteUserAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        // POST /api/Users/{userId}/creatures
        public async Task<bool> AssignCreatureToUserAsync(string userId, UserCreatureDto creatureDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/{userId}/creatures", creatureDto);
            return response.IsSuccessStatusCode;
        }

        // POST /api/Users/{userId}/quests
        public async Task<bool> AssignQuestToUserAsync(string userId, UserQuestDto questDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/{userId}/quests", questDto);
            return response.IsSuccessStatusCode;
        }

        // PATCH /api/Users/{userId}/quests/{questId}/complete
        public async Task<bool> CompleteUserQuestAsync(string userId, string questId)
        {
            var response = await _httpClient.PatchAsync($"{_baseUrl}/{userId}/quests/{questId}/complete", null);
            return response.IsSuccessStatusCode;
        }
    }
}
