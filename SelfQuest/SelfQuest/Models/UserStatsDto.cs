namespace SelfQuest.Models
{
    public class UserStatsDto
    {
        public int TotalQuestsCompleted { get; set; } = 0;
        public int TotalXpEarned { get; set; } = 0;
        public int DaysActive { get; set; } = 0;
        public string LastLoginAt { get; set; } = string.Empty;
    }
}