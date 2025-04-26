using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using MongoDB.Driver;
using TaskModel = KindQuestAPI.Models.Task; // Alias to avoid naming conflicts

namespace KindQuestAPI.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<TaskModel> _tasks;

        public TaskRepository(IMongoDatabase database)
        {
            _tasks = database.GetCollection<TaskModel>("Tasks");
        }

        public async Task<List<TaskModel>> GetAllAsync()
        {
            return await _tasks.Find(_ => true).ToListAsync();
        }

        public async Task<TaskModel> GetByIdAsync(string id)
        {
            return await _tasks.Find(task => task.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<TaskModel>> GetByAssignedToAsync(string userId)
        {
            return await _tasks.Find(task => task.AssignedTo == userId).ToListAsync();
        }

        public async Task<List<TaskModel>> GetByStatusAsync(string status)
        {
            return await _tasks.Find(task => task.Status == status).ToListAsync();
        }

        public async Task<List<TaskModel>> GetByPriorityAsync(string priority)
        {
            return await _tasks.Find(task => task.Priority == priority).ToListAsync();
        }

        public async Task<List<TaskModel>> GetSystemTasksAsync()
        {
            return await _tasks.Find(task => task.IsSystemTask == true).ToListAsync();
        }

        public async Task<List<TaskModel>> GetUserTasksAsync(string userId)
        {
            return await _tasks.Find(task => task.CreatedBy == userId && task.IsSystemTask == false).ToListAsync();
        }

        public async Task<bool> CreateAsync(TaskModel task)
        {
            try
            {
                await _tasks.InsertOneAsync(task);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, TaskModel task)
        {
            try
            {
                var result = await _tasks.ReplaceOneAsync(t => t.Id == id, task);
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
                var result = await _tasks.DeleteOneAsync(task => task.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateStatusAsync(string id, string status)
        {
            try
            {
                var filter = Builders<TaskModel>.Filter.Eq(t => t.Id, id);
                var update = Builders<TaskModel>.Update.Set(t => t.Status, status);

                var result = await _tasks.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CompleteTaskAsync(string id)
        {
            try
            {
                var filter = Builders<TaskModel>.Filter.Eq(t => t.Id, id);
                var update = Builders<TaskModel>.Update
                    .Set(t => t.Status, "completed")
                    .Set(t => t.CompletedAt, DateTime.UtcNow.ToString("o"));

                var result = await _tasks.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}