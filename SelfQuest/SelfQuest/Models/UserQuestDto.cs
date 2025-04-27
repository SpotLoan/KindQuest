// SelfQuest/Models/UserQuestDto.cs
namespace SelfQuest.Models
{
    public class UserQuestDto
    {
        public string QuestId { get; set; } = null!;
        public string CreatureId { get; set; } = null!;
        public int Progress { get; set; } = 0;
        public int MaxProgress { get; set; } = 1;
        public bool IsCompleted { get; set; } = false;
        public string AssignedAt { get; set; } = string.Empty;
        public string ExpiresAt { get; set; } = string.Empty;
        public string CompletedAt { get; set; } = string.Empty;
    }
}