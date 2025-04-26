using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.DAL.Repositories
{
    public class CreatureRepository : ICreatureRepository
    {
        private readonly IMongoCollection<Creatures> _creatures;

        public CreatureRepository(IMongoDatabase database)
        {
            _creatures = database.GetCollection<Creatures>("Creatures");
        }

        public async Task<List<Creatures>> GetAllAsync()
        {
            return await _creatures.Find(_ => true).ToListAsync();
        }

        public async Task<Creatures> GetByIdAsync(string id)
        {
            return await _creatures.Find(creature => creature.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Creatures>> GetByGenreAsync(string genre)
        {
            return await _creatures.Find(creature => creature.Genre == genre).ToListAsync();
        }

        public async Task<List<Creatures>> GetByRarityAsync(string rarity)
        {
            return await _creatures.Find(creature => creature.Rarity == rarity).ToListAsync();
        }

        public async Task<bool> CreateAsync(Creatures creature)
        {
            try
            {
                await _creatures.InsertOneAsync(creature);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, Creatures creature)
        {
            try
            {
                var result = await _creatures.ReplaceOneAsync(c => c.Id == id, creature);
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
                var result = await _creatures.DeleteOneAsync(creature => creature.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddQuestToPoolAsync(string creatureId, string questId)
        {
            try
            {
                var filter = Builders<Creatures>.Filter.Eq(c => c.Id, creatureId);
                var update = Builders<Creatures>.Update.AddToSet(c => c.QuestsPool, questId);

                var result = await _creatures.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveQuestFromPoolAsync(string creatureId, string questId)
        {
            try
            {
                var filter = Builders<Creatures>.Filter.Eq(c => c.Id, creatureId);
                var update = Builders<Creatures>.Update.Pull(c => c.QuestsPool, questId);

                var result = await _creatures.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}