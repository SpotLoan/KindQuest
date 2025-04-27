using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class CreatureAnimations
{
    [BsonElement("idle")]
    public string Idle { get; set; } = string.Empty;

    [BsonElement("happy")]
    public string Happy { get; set; } = string.Empty;

    [BsonElement("sad")]
    public string Sad { get; set; } = string.Empty;
}