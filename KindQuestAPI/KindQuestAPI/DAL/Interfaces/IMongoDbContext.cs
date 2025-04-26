using KindQuestAPI.Models;
using MongoDB.Driver;
using TaskModel = KindQuestAPI.Models.Task; // Use consistent alias

namespace KindQuestAPI.DAL.Interfaces;

public interface IMongoDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<Creatures> Creatures { get; }
    IMongoCollection<Quest> Quests { get; }
    IMongoCollection<Item> Items { get; }
    IMongoCollection<Post> Posts { get; }
    IMongoCollection<TaskModel> Tasks { get; } // Use TaskModel instead of Task
}