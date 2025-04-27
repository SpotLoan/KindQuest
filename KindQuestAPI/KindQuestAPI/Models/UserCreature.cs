using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class UserCreature
{
    [BsonElement("creatureId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatureId { get; set; } = null!;

    [BsonElement("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [BsonElement("acquired")]
    public string Acquired { get; set; } = DateTime.UtcNow.ToString("o");

    [BsonElement("xp")]
    public int Xp { get; set; } = 0;

    [BsonElement("level")]
    public int Level { get; set; } = 1;

    [BsonElement("favorite")]
    public bool Favorite { get; set; } = false;
}