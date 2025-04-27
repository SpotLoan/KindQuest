using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KindQuestAPI.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

    [BsonElement("likes")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Likes { get; set; } = new List<string>();

    [BsonElement("comments")]
    public List<PostComment> Comments { get; set; } = new List<PostComment>();

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new List<string>();

    [BsonElement("relatedCreatures")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> RelatedCreatures { get; set; } = new List<string>();
}