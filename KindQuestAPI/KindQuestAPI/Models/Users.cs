using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace KindQuestAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("bio")]
        public string Bio { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        [BsonElement("updatedAt")]
        public string UpdatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        [BsonElement("profileImageUrl")]
        public string ProfileImageUrl { get; set; } = string.Empty;

        [BsonElement("followers")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Followers { get; set; } = new List<string>();

        [BsonElement("following")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Following { get; set; } = new List<string>();

        [BsonElement("creatures")]
        public List<UserCreature> Creatures { get; set; } = new List<UserCreature>();

        [BsonElement("activeQuests")]
        public List<UserQuest> ActiveQuests { get; set; } = new List<UserQuest>();

        [BsonElement("stats")]
        public UserStats Stats { get; set; } = new UserStats();
    }
}