using KindQuestAPI.Models;

namespace KindQuestAPI.DAL.Interfaces;

public interface IItemRepository
{
    Task<List<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(string id);
    Task<List<Item>> GetByTypeAsync(string type);
    Task<List<Item>> GetByRarityAsync(string rarity);
    Task<bool> CreateAsync(Item item);
    Task<bool> UpdateAsync(string id, Item item);
    Task<bool> DeleteAsync(string id);
}