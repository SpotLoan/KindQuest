using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class ItemEffect
{
    [BsonElement("stat")]
    public string Stat { get; set; } = string.Empty;

    [BsonElement("value")]
    public int Value { get; set; } = 0;

    [BsonElement("duration")]
    public int Duration { get; set; } = 0;
}