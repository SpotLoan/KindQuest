using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.DAL.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMongoCollection<Item> _items;

        public ItemRepository(IMongoDatabase database)
        {
            _items = database.GetCollection<Item>("Items");
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _items.Find(_ => true).ToListAsync();
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            return await _items.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Item>> GetByTypeAsync(string type)
        {
            return await _items.Find(item => item.Type == type).ToListAsync();
        }

        public async Task<List<Item>> GetByRarityAsync(string rarity)
        {
            return await _items.Find(item => item.Rarity == rarity).ToListAsync();
        }

        public async Task<bool> CreateAsync(Item item)
        {
            try
            {
                await _items.InsertOneAsync(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, Item item)
        {
            try
            {
                var result = await _items.ReplaceOneAsync(i => i.Id == id, item);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var result = await _items.DeleteOneAsync(item => item.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}