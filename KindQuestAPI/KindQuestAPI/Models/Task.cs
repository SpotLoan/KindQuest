using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class Task
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("title")]
    public string Title { get; set; } = null!;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("isSystemTask")]
    public bool IsSystemTask { get; set; } = false;

    [BsonElement("assignedTo")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AssignedTo { get; set; } = null!;

    [BsonElement("createdBy")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CreatedBy { get; set; }

    [BsonElement("createdAt")]
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

    [BsonElement("dueDate")]
    public string DueDate { get; set; } = DateTime.UtcNow.AddDays(7).ToString("o");

    [BsonElement("priority")]
    public string Priority { get; set; } = "medium";

    [BsonElement("status")]
    public string Status { get; set; } = "open";

    [BsonElement("completedAt")]
    public string? CompletedAt { get; set; }

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new List<string>();
}