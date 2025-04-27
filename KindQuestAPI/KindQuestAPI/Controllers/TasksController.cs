using KindQuestAPI.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskModel = KindQuestAPI.Models.Task; // Alias to avoid conflict

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return Ok(tasks);
        }

        // GET: api/Tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTask(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // GET: api/Tasks/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksByUser(string userId)
        {
            var tasks = await _taskRepository.GetByAssignedToAsync(userId);
            return Ok(tasks);
        }

        // GET: api/Tasks/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksByStatus(string status)
        {
            var tasks = await _taskRepository.GetByStatusAsync(status);
            return Ok(tasks);
        }

        // GET: api/Tasks/priority/{priority}
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksByPriority(string priority)
        {
            var tasks = await _taskRepository.GetByPriorityAsync(priority);
            return Ok(tasks);
        }

        // GET: api/Tasks/system
        [HttpGet("system")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetSystemTasks()
        {
            var tasks = await _taskRepository.GetSystemTasksAsync();
            return Ok(tasks);
        }

        // GET: api/Tasks/user-created/{userId}
        [HttpGet("user-created/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetUserCreatedTasks(string userId)
        {
            var tasks = await _taskRepository.GetUserTasksAsync(userId);
            return Ok(tasks);
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskModel>> CreateTask(TaskModel task)
        {
            var result = await _taskRepository.CreateAsync(task);

            if (!result)
            {
                return BadRequest("Failed to create task");
            }

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/Tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, TaskModel task)
        {
            if (id != task.Id)
            {
                return BadRequest("Task ID mismatch");
            }

            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            var result = await _taskRepository.UpdateAsync(id, task);
            if (!result)
            {
                return StatusCode(500, "Failed to update task");
            }

            return NoContent();
        }

        // DELETE: api/Tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var result = await _taskRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete task");
            }

            return NoContent();
        }

        // PATCH: api/Tasks/{id}/status/{status}
        [HttpPatch("{id}/status/{status}")]
        public async Task<IActionResult> UpdateTaskStatus(string id, string status)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var result = await _taskRepository.UpdateStatusAsync(id, status);
            if (!result)
            {
                return StatusCode(500, "Failed to update task status");
            }

            return Ok();
        }

        // PATCH: api/Tasks/{id}/complete
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteTask(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var result = await _taskRepository.CompleteTaskAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to complete task");
            }

            return Ok();
        }
    }
}