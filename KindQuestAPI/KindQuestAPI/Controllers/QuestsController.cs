using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestsController : ControllerBase
    {
        private readonly IQuestRepository _questRepository;

        public QuestsController(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        // GET: api/Quests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuests()
        {
            var quests = await _questRepository.GetAllAsync();
            return Ok(quests);
        }

        // GET: api/Quests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Quest>> GetQuest(string id)
        {
            var quest = await _questRepository.GetByIdAsync(id);

            if (quest == null)
            {
                return NotFound();
            }

            return Ok(quest);
        }

        // GET: api/Quests/type/{type}
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuestsByType(string type)
        {
            var quests = await _questRepository.GetByTypeAsync(type);
            return Ok(quests);
        }

        // GET: api/Quests/genre/{genre}
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuestsByGenre(string genre)
        {
            var quests = await _questRepository.GetByGenreAsync(genre);
            return Ok(quests);
        }

        // GET: api/Quests/difficulty/{difficulty}
        [HttpGet("difficulty/{difficulty}")]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuestsByDifficulty(string difficulty)
        {
            var quests = await _questRepository.GetByDifficultyAsync(difficulty);
            return Ok(quests);
        }

        // GET: api/Quests/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Quest>>> GetActiveQuests()
        {
            var quests = await _questRepository.GetActiveQuestsAsync();
            return Ok(quests);
        }

        // POST: api/Quests
        [HttpPost]
        public async Task<ActionResult<Quest>> CreateQuest(Quest quest)
        {
            var result = await _questRepository.CreateAsync(quest);

            if (!result)
            {
                return BadRequest("Failed to create quest");
            }

            return CreatedAtAction(nameof(GetQuest), new { id = quest.Id }, quest);
        }

        // PUT: api/Quests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuest(string id, Quest quest)
        {
            if (id != quest.Id)
            {
                return BadRequest("Quest ID mismatch");
            }

            var existingQuest = await _questRepository.GetByIdAsync(id);
            if (existingQuest == null)
            {
                return NotFound();
            }

            var result = await _questRepository.UpdateAsync(id, quest);
            if (!result)
            {
                return StatusCode(500, "Failed to update quest");
            }

            return NoContent();
        }

        // DELETE: api/Quests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuest(string id)
        {
            var quest = await _questRepository.GetByIdAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            var result = await _questRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete quest");
            }

            return NoContent();
        }

        // PATCH: api/Quests/{id}/activate
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateQuest(string id)
        {
            var quest = await _questRepository.GetByIdAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            var result = await _questRepository.ActivateQuestAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to activate quest");
            }

            return Ok();
        }

        // PATCH: api/Quests/{id}/deactivate
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateQuest(string id)
        {
            var quest = await _questRepository.GetByIdAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            var result = await _questRepository.DeactivateQuestAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to deactivate quest");
            }

            return Ok();
        }

        // POST: api/Quests/tags
        [HttpPost("tags")]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuestsByTags([FromBody] List<string> tags)
        {
            var quests = await _questRepository.GetByTagsAsync(tags);
            return Ok(quests);
        }
    }
}