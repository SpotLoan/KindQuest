using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreaturesController : ControllerBase
    {
        private readonly ICreatureRepository _creatureRepository;

        public CreaturesController(ICreatureRepository creatureRepository)
        {
            _creatureRepository = creatureRepository;
        }

        // GET: api/Creatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Creatures>>> GetCreatures()
        {
            var creatures = await _creatureRepository.GetAllAsync();
            return Ok(creatures);
        }

        // GET: api/Creatures/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Creatures>> GetCreature(string id)
        {
            var creature = await _creatureRepository.GetByIdAsync(id);

            if (creature == null)
            {
                return NotFound();
            }

            return Ok(creature);
        }

        // GET: api/Creatures/genre/{genre}
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<Creatures>>> GetCreaturesByGenre(string genre)
        {
            var creatures = await _creatureRepository.GetByGenreAsync(genre);
            return Ok(creatures);
        }

        // GET: api/Creatures/rarity/{rarity}
        [HttpGet("rarity/{rarity}")]
        public async Task<ActionResult<IEnumerable<Creatures>>> GetCreaturesByRarity(string rarity)
        {
            var creatures = await _creatureRepository.GetByRarityAsync(rarity);
            return Ok(creatures);
        }

        // POST: api/Creatures
        [HttpPost]
        public async Task<ActionResult<Creatures>> CreateCreature(Creatures creature)
        {
            var result = await _creatureRepository.CreateAsync(creature);

            if (!result)
            {
                return BadRequest("Failed to create creature");
            }

            return CreatedAtAction(nameof(GetCreature), new { id = creature.Id }, creature);
        }

        // PUT: api/Creatures/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCreature(string id, Creatures creature)
        {
            if (id != creature.Id)
            {
                return BadRequest("Creature ID mismatch");
            }

            var existingCreature = await _creatureRepository.GetByIdAsync(id);
            if (existingCreature == null)
            {
                return NotFound();
            }

            var result = await _creatureRepository.UpdateAsync(id, creature);
            if (!result)
            {
                return StatusCode(500, "Failed to update creature");
            }

            return NoContent();
        }

        // DELETE: api/Creatures/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreature(string id)
        {
            var creature = await _creatureRepository.GetByIdAsync(id);
            if (creature == null)
            {
                return NotFound();
            }

            var result = await _creatureRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete creature");
            }

            return NoContent();
        }

        // POST: api/Creatures/{id}/quests/{questId}
        [HttpPost("{id}/quests/{questId}")]
        public async Task<IActionResult> AddQuestToCreature(string id, string questId)
        {
            var creature = await _creatureRepository.GetByIdAsync(id);
            if (creature == null)
            {
                return NotFound("Creature not found");
            }

            var result = await _creatureRepository.AddQuestToPoolAsync(id, questId);
            if (!result)
            {
                return StatusCode(500, "Failed to add quest to creature's pool");
            }

            return Ok();
        }

        // DELETE: api/Creatures/{id}/quests/{questId}
        [HttpDelete("{id}/quests/{questId}")]
        public async Task<IActionResult> RemoveQuestFromCreature(string id, string questId)
        {
            var creature = await _creatureRepository.GetByIdAsync(id);
            if (creature == null)
            {
                return NotFound("Creature not found");
            }

            var result = await _creatureRepository.RemoveQuestFromPoolAsync(id, questId);
            if (!result)
            {
                return StatusCode(500, "Failed to remove quest from creature's pool");
            }

            return Ok();
        }
    }
}