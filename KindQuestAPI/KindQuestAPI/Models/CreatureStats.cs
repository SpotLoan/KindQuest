using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class CreatureStats
{
    [BsonElement("health")]
    public int Health { get; set; } = 50;

    [BsonElement("strength")]
    public int Strength { get; set; } = 50;

    [BsonElement("intelligence")]
    public int Intelligence { get; set; } = 50;

    [BsonElement("speed")]
    public int Speed { get; set; } = 50;
}