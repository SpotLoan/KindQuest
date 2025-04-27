namespace SelfQuest.Models
{
    public class UserCreatureDto
    {
        public string CreatureId { get; set; } = null!;
        public string Nickname { get; set; } = string.Empty;
        public string Acquired { get; set; } = string.Empty;
        public int Xp { get; set; } = 0;
        public int Level { get; set; } = 1;
        public bool Favorite { get; set; } = false;
    }
}