using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = "consumable";

    [BsonElement("rarity")]
    public string Rarity { get; set; } = "common";

    [BsonElement("effects")]
    public List<ItemEffect> Effects { get; set; } = new List<ItemEffect>();

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("price")]
    public int Price { get; set; } = 100;

    [BsonElement("tradeable")]
    public bool Tradeable { get; set; } = true;

    [BsonElement("createdAt")]
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
}