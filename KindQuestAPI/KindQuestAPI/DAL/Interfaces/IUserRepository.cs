using KindQuestAPI.Models;

namespace KindQuestAPI.DAL.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<bool> CreateAsync(User user);
    Task<bool> UpdateAsync(string id, User user);
    Task<bool> DeleteAsync(string id);
    Task<bool> AddCreatureToUserAsync(string userId, UserCreature creature);
    Task<bool> UpdateUserCreatureAsync(string userId, string creatureId, UserCreature updatedCreature);
    Task<bool> RemoveCreatureFromUserAsync(string userId, string creatureId);
    Task<bool> AddQuestToUserAsync(string userId, UserQuest quest);
    Task<bool> UpdateUserQuestAsync(string userId, string questId, UserQuest updatedQuest);
    Task<bool> CompleteUserQuestAsync(string userId, string questId);
    Task<bool> UpdateUserStatsAsync(string userId, UserStats stats);
}