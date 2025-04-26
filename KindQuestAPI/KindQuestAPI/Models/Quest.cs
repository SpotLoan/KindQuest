using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class Quest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("title")]
    public string Title { get; set; } = null!;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = "daily";

    [BsonElement("difficulty")]
    public string Difficulty { get; set; } = "normal";

    [BsonElement("createdAt")]
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

    [BsonElement("requirementType")]
    public string RequirementType { get; set; } = "counter";

    [BsonElement("requirementValue")]
    public int RequirementValue { get; set; } = 1;

    [BsonElement("requirementDescription")]
    public string RequirementDescription { get; set; } = string.Empty;

    [BsonElement("rewards")]
    public QuestRewards Rewards { get; set; } = new QuestRewards();

    [BsonElement("genre")]
    public string Genre { get; set; } = "general";

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new List<string>();

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("prerequisiteQuests")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> PrerequisiteQuests { get; set; } = new List<string>();
}