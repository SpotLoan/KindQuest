using KindQuestAPI.Models;

namespace KindQuestAPI.DAL.Interfaces;

public interface IQuestRepository
{
    Task<List<Quest>> GetAllAsync();
    Task<Quest> GetByIdAsync(string id);
    Task<List<Quest>> GetByTypeAsync(string type);
    Task<List<Quest>> GetByGenreAsync(string genre);
    Task<List<Quest>> GetByDifficultyAsync(string difficulty);
    Task<List<Quest>> GetByTagsAsync(List<string> tags);
    Task<List<Quest>> GetActiveQuestsAsync();
    Task<bool> CreateAsync(Quest quest);
    Task<bool> UpdateAsync(string id, Quest quest);
    Task<bool> DeleteAsync(string id);
    Task<bool> ActivateQuestAsync(string id);
    Task<bool> DeactivateQuestAsync(string id);
}