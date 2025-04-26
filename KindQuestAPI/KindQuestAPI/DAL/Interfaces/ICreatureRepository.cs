using KindQuestAPI.Models;

namespace KindQuestAPI.DAL.Interfaces;

public interface ICreatureRepository
{
    Task<List<Creatures>> GetAllAsync();
    Task<Creatures> GetByIdAsync(string id);
    Task<List<Creatures>> GetByGenreAsync(string genre);
    Task<List<Creatures>> GetByRarityAsync(string rarity);
    Task<bool> CreateAsync(Creatures creature);
    Task<bool> UpdateAsync(string id, Creatures creature);
    Task<bool> DeleteAsync(string id);
    Task<bool> AddQuestToPoolAsync(string creatureId, string questId);
    Task<bool> RemoveQuestFromPoolAsync(string creatureId, string questId);
}