namespace SelfQuest.Models;
public class PostCommentDto
{
    public string UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}