using KindQuestAPI.Configuration;
using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskModel = KindQuestAPI.Models.Task; // Use consistent alias

namespace KindQuestAPI.DAL
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Creatures> Creatures => _database.GetCollection<Creatures>("Creatures");
        public IMongoCollection<Quest> Quests => _database.GetCollection<Quest>("Quests");
        public IMongoCollection<Item> Items => _database.GetCollection<Item>("Items");
        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
        public IMongoCollection<TaskModel> Tasks => _database.GetCollection<TaskModel>("Tasks");
    }
}