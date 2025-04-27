using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var result = await _userRepository.CreateAsync(user);

            if (!result)
            {
                return BadRequest("Failed to create user");
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch");
            }

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            var result = await _userRepository.UpdateAsync(id, user);
            if (!result)
            {
                return StatusCode(500, "Failed to update user");
            }

            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete user");
            }

            return NoContent();
        }

        // POST: api/Users/{userId}/creatures
        [HttpPost("{userId}/creatures")]
        public async Task<IActionResult> AddCreatureToUser(string userId, UserCreature creature)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userRepository.AddCreatureToUserAsync(userId, creature);
            if (!result)
            {
                return StatusCode(500, "Failed to add creature to user");
            }

            return Ok();
        }

        // POST: api/Users/{userId}/quests
        [HttpPost("{userId}/quests")]
        public async Task<IActionResult> AddQuestToUser(string userId, UserQuest quest)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userRepository.AddQuestToUserAsync(userId, quest);
            if (!result)
            {
                return StatusCode(500, "Failed to add quest to user");
            }

            return Ok();
        }

        // PATCH: api/Users/{userId}/quests/{questId}/complete
        [HttpPatch("{userId}/quests/{questId}/complete")]
        public async Task<IActionResult> CompleteQuest(string userId, string questId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var quest = user.ActiveQuests.Find(q => q.QuestId == questId);
            if (quest == null)
            {
                return NotFound("Quest not found");
            }

            var result = await _userRepository.CompleteUserQuestAsync(userId, questId);
            if (!result)
            {
                return StatusCode(500, "Failed to complete quest");
            }

            return Ok();
        }
    }
}