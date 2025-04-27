using SelfQuest.Models;

namespace SelfQuest.Services
{
    public interface ICreatureService
    {
        Task<List<CreatureDto>> GetAllCreaturesAsync();
        Task<CreatureDto> GetCreatureByIdAsync(string id);
        Task<List<CreatureDto>> GetCreaturesByGenreAsync(string genre);
        Task<List<CreatureDto>> GetCreaturesByRarityAsync(string rarity);
        // Add create, update, delete if you want those features!
    }
}