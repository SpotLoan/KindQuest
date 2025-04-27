using System.Net.Http;
using System.Net.Http.Json;
using SelfQuest.Models;

namespace SelfQuest.Services
{
    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Posts";

        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PostDto>>(_baseUrl);
        }

        public async Task<PostDto> GetPostByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<PostDto>($"{_baseUrl}/{id}");
        }

        public async Task<PostDto> CreatePostAsync(PostDto post)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, post);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PostDto>();
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
