using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KindQuestAPI.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, User user)
        {
            try
            {
                user.UpdatedAt = DateTime.UtcNow.ToString("o");
                var result = await _users.ReplaceOneAsync(u => u.Id == id, user);
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
                var result = await _users.DeleteOneAsync(user => user.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddCreatureToUserAsync(string userId, UserCreature creature)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var update = Builders<User>.Update.Push(u => u.Creatures, creature)
                                                 .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString("o"));

                var result = await _users.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserCreatureAsync(string userId, string creatureId, UserCreature updatedCreature)
        {
            try
            {
                // Find the user and get their creature array
                var user = await GetByIdAsync(userId);
                if (user == null) return false;

                // Find and update the specific creature
                var creatureIndex = user.Creatures.FindIndex(c => c.CreatureId == creatureId);
                if (creatureIndex == -1) return false;

                // Update the creature
                user.Creatures[creatureIndex] = updatedCreature;
                user.UpdatedAt = DateTime.UtcNow.ToString("o");

                // Replace the entire user document
                var result = await _users.ReplaceOneAsync(u => u.Id == userId, user);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCreatureFromUserAsync(string userId, string creatureId)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var update = Builders<User>.Update.PullFilter(
                                u => u.Creatures, 
                                c => c.CreatureId == creatureId)
                            .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString("o"));

                var result = await _users.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddQuestToUserAsync(string userId, UserQuest quest)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var update = Builders<User>.Update.Push(u => u.ActiveQuests, quest)
                                                 .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString("o"));

                var result = await _users.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserQuestAsync(string userId, string questId, UserQuest updatedQuest)
        {
            try
            {
                // Find the user and get their active quests array
                var user = await GetByIdAsync(userId);
                if (user == null) return false;

                // Find and update the specific quest
                var questIndex = user.ActiveQuests.FindIndex(q => q.QuestId == questId);
                if (questIndex == -1) return false;

                // Update the quest
                user.ActiveQuests[questIndex] = updatedQuest;
                user.UpdatedAt = DateTime.UtcNow.ToString("o");

                // Replace the entire user document
                var result = await _users.ReplaceOneAsync(u => u.Id == userId, user);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CompleteUserQuestAsync(string userId, string questId)
        {
            try
            {
                // Find the user and get their active quests array
                var user = await GetByIdAsync(userId);
                if (user == null) return false;

                // Find the specific quest
                var questIndex = user.ActiveQuests.FindIndex(q => q.QuestId == questId);
                if (questIndex == -1) return false;

                // Mark the quest as completed
                user.ActiveQuests[questIndex].IsCompleted = true;
                user.ActiveQuests[questIndex].CompletedAt = DateTime.UtcNow.ToString("o");
                user.UpdatedAt = DateTime.UtcNow.ToString("o");

                // Update user stats
                user.Stats.TotalQuestsCompleted++;

                // Replace the entire user document
                var result = await _users.ReplaceOneAsync(u => u.Id == userId, user);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserStatsAsync(string userId, UserStats stats)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var update = Builders<User>.Update.Set(u => u.Stats, stats)
                                                 .Set(u => u.UpdatedAt, DateTime.UtcNow.ToString("o"));

                var result = await _users.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}