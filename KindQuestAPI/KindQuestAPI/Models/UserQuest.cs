using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class UserQuest
{
    [BsonElement("questId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string QuestId { get; set; } = null!;

    [BsonElement("creatureId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatureId { get; set; } = null!;

    [BsonElement("progress")]
    public int Progress { get; set; } = 0;

    [BsonElement("maxProgress")]
    public int MaxProgress { get; set; } = 1;

    [BsonElement("isCompleted")]
    public bool IsCompleted { get; set; } = false;

    [BsonElement("assignedAt")]
    public string AssignedAt { get; set; } = DateTime.UtcNow.ToString("o");

    [BsonElement("expiresAt")]
    public string ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(1).ToString("o");

    [BsonElement("completedAt")]
    public string CompletedAt { get; set; } = string.Empty;
}