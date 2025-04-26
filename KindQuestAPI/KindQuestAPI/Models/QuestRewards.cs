using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class QuestRewards
{
    [BsonElement("xp")]
    public int Xp { get; set; } = 10;

    [BsonElement("coins")]
    public int Coins { get; set; } = 0;

    [BsonElement("items")]
    public List<QuestRewardItem> Items { get; set; } = new List<QuestRewardItem>();
}