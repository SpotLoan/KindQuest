using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace KindQuestAPI.Models
{
    public class Creatures
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        [BsonElement("genre")]
        public string Genre { get; set; } = "fantasy";

        [BsonElement("rarity")]
        public string Rarity { get; set; } = "common";

        [BsonElement("baseStats")]
        public CreatureStats BaseStats { get; set; } = new CreatureStats();

        [BsonElement("questsPool")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> QuestsPool { get; set; } = new List<string>();

        [BsonElement("associatedTraits")]
        public List<string> AssociatedTraits { get; set; } = new List<string>();

        [BsonElement("evolutionPath")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> EvolutionPath { get; set; } = new List<string>();

        [BsonElement("evolutionRequirements")]
        public EvolutionRequirements EvolutionRequirements { get; set; } = new EvolutionRequirements();

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [BsonElement("animations")]
        public CreatureAnimations Animations { get; set; } = new CreatureAnimations();
    }
}