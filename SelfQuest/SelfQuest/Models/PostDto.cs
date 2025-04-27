namespace SelfQuest.Models;

public class PostDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string AuthorUsername { get; set; } // Optional, if you want to show username
    public string Content { get; set; }
    public string ImageUrl { get; set; } // <-- Add this!
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Likes { get; set; }
    public List<PostCommentDto> Comments { get; set; }
    public List<string> RelatedCreatures { get; set; }
}