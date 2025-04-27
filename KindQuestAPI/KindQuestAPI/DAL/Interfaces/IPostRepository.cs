using KindQuestAPI.Models;

namespace KindQuestAPI.DAL.Interfaces;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task<Post> GetByIdAsync(string id);
    Task<List<Post>> GetByUserIdAsync(string userId);
    Task<List<Post>> GetByTagsAsync(List<string> tags);
    Task<bool> CreateAsync(Post post);
    Task<bool> UpdateAsync(string id, Post post);
    Task<bool> DeleteAsync(string id);
    Task<bool> AddCommentAsync(string postId, PostComment comment);
    Task<bool> RemoveCommentAsync(string postId, string userId, string createdAt);
    Task<bool> AddLikeAsync(string postId, string userId);
    Task<bool> RemoveLikeAsync(string postId, string userId);
}