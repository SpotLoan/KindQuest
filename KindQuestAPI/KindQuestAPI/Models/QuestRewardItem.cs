using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class QuestRewardItem
{
    [BsonElement("itemId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ItemId { get; set; } = null!;

    [BsonElement("quantity")]
    public int Quantity { get; set; } = 1;

    [BsonElement("dropChance")]
    public double DropChance { get; set; } = 1.0;
}