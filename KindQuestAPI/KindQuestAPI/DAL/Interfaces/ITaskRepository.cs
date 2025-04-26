using System.Collections.Generic;
using System.Threading.Tasks;
using TaskModel = KindQuestAPI.Models.Task; // Alias to avoid naming conflicts

namespace KindQuestAPI.DAL.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetAllAsync();
        Task<TaskModel> GetByIdAsync(string id);
        Task<List<TaskModel>> GetByAssignedToAsync(string userId);
        Task<List<TaskModel>> GetByStatusAsync(string status);
        Task<List<TaskModel>> GetByPriorityAsync(string priority);
        Task<List<TaskModel>> GetSystemTasksAsync();
        Task<List<TaskModel>> GetUserTasksAsync(string userId);
        Task<bool> CreateAsync(TaskModel task);
        Task<bool> UpdateAsync(string id, TaskModel task);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string id, string status);
        Task<bool> CompleteTaskAsync(string id);
    }
}