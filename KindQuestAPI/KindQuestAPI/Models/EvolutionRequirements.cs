using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class EvolutionRequirements
{
    [BsonElement("minLevel")]
    public int MinLevel { get; set; } = 10;

    [BsonElement("minXp")]
    public int MinXp { get; set; } = 500;

    [BsonElement("specialItems")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> SpecialItems { get; set; } = new List<string>();
}