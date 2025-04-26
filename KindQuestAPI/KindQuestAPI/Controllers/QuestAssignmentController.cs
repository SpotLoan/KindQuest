using KindQuestAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestAssignmentController : ControllerBase
    {
        private readonly QuestService _questService;

        public QuestAssignmentController(QuestService questService)
        {
            _questService = questService;
        }

        // POST: api/QuestAssignment/user/{userId}
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> AssignQuestsToUser(string userId)
        {
            var result = await _questService.AssignDailyQuestsToUserAsync(userId);
            
            if (!result)
            {
                return StatusCode(500, "Failed to assign quests to user");
            }
            
            return Ok("Quests assigned successfully");
        }

        // POST: api/QuestAssignment/all
        [HttpPost("all")]
        public async Task<IActionResult> AssignQuestsToAllUsers()
        {
            var count = await _questService.AssignDailyQuestsToAllUsersAsync();
            return Ok($"Quests assigned to {count} users");
        }

        // PATCH: api/QuestAssignment/user/{userId}/quest/{questId}/progress/{progress}
        [HttpPatch("user/{userId}/quest/{questId}/progress/{progress}")]
        public async Task<IActionResult> UpdateQuestProgress(string userId, string questId, int progress)
        {
            var result = await _questService.UpdateQuestProgressAsync(userId, questId, progress);
            
            if (!result)
            {
                return StatusCode(500, "Failed to update quest progress");
            }
            
            return Ok("Quest progress updated successfully");
        }
    }
}