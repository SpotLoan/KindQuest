namespace SelfQuest.Models
{
    public class CreatureDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public string ImageUrl { get; set; }
        public string Genre { get; set; }
        public string Rarity { get; set; }
        public bool Favorite { get; set; }
        // Add more properties if your API returns them!
    }
}