using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.DAL.Repositories
{
    public class QuestRepository : IQuestRepository
    {
        private readonly IMongoCollection<Quest> _quests;

        public QuestRepository(IMongoDatabase database)
        {
            _quests = database.GetCollection<Quest>("Quests");
        }

        public async Task<List<Quest>> GetAllAsync()
        {
            return await _quests.Find(_ => true).ToListAsync();
        }

        public async Task<Quest> GetByIdAsync(string id)
        {
            return await _quests.Find(quest => quest.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Quest>> GetByTypeAsync(string type)
        {
            return await _quests.Find(quest => quest.Type == type).ToListAsync();
        }

        public async Task<List<Quest>> GetByGenreAsync(string genre)
        {
            return await _quests.Find(quest => quest.Genre == genre).ToListAsync();
        }

        public async Task<List<Quest>> GetByDifficultyAsync(string difficulty)
        {
            return await _quests.Find(quest => quest.Difficulty == difficulty).ToListAsync();
        }

        public async Task<List<Quest>> GetByTagsAsync(List<string> tags)
        {
            // Find quests that have at least one matching tag
            var filter = Builders<Quest>.Filter.AnyIn(q => q.Tags, tags);
            return await _quests.Find(filter).ToListAsync();
        }

        public async Task<List<Quest>> GetActiveQuestsAsync()
        {
            return await _quests.Find(quest => quest.IsActive == true).ToListAsync();
        }

        public async Task<bool> CreateAsync(Quest quest)
        {
            try
            {
                await _quests.InsertOneAsync(quest);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, Quest quest)
        {
            try
            {
                var result = await _quests.ReplaceOneAsync(q => q.Id == id, quest);
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
                var result = await _quests.DeleteOneAsync(quest => quest.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ActivateQuestAsync(string id)
        {
            try
            {
                var filter = Builders<Quest>.Filter.Eq(q => q.Id, id);
                var update = Builders<Quest>.Update.Set(q => q.IsActive, true);

                var result = await _quests.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeactivateQuestAsync(string id)
        {
            try
            {
                var filter = Builders<Quest>.Filter.Eq(q => q.Id, id);
                var update = Builders<Quest>.Update.Set(q => q.IsActive, false);

                var result = await _quests.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}