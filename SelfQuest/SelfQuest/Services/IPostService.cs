using SelfQuest.Models;

namespace SelfQuest.Services;

public interface IPostService
{
    Task<List<PostDto>> GetAllPostsAsync();
    Task<PostDto> GetPostByIdAsync(string id);
    Task<PostDto> CreatePostAsync(PostDto post);
    Task<bool> DeletePostAsync(string id);
    // Add more as needed (e.g., comments, likes)
}