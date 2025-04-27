using System.Net.Http;
using System.Net.Http.Json;
using SelfQuest.Models;

namespace SelfQuest.Services
{
    public class CreatureService : ICreatureService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Creatures";

        public CreatureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CreatureDto>> GetAllCreaturesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CreatureDto>>(_baseUrl);
        }

        public async Task<CreatureDto> GetCreatureByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<CreatureDto>($"{_baseUrl}/{id}");
        }

        public async Task<List<CreatureDto>> GetCreaturesByGenreAsync(string genre)
        {
            return await _httpClient.GetFromJsonAsync<List<CreatureDto>>($"{_baseUrl}/genre/{genre}");
        }

        public async Task<List<CreatureDto>> GetCreaturesByRarityAsync(string rarity)
        {
            return await _httpClient.GetFromJsonAsync<List<CreatureDto>>($"{_baseUrl}/rarity/{rarity}");
        }
    }
}