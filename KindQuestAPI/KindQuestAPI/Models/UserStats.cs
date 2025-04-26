using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class UserStats
{
    [BsonElement("totalQuestsCompleted")]
    public int TotalQuestsCompleted { get; set; } = 0;

    [BsonElement("totalXpEarned")]
    public int TotalXpEarned { get; set; } = 0;

    [BsonElement("daysActive")]
    public int DaysActive { get; set; } = 0;

    [BsonElement("lastLoginAt")]
    public string LastLoginAt { get; set; } = DateTime.UtcNow.ToString("o");
}