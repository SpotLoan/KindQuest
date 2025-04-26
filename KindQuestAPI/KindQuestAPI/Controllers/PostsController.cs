using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            return Ok(posts);
        }

        // GET: api/Posts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(string id)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // GET: api/Posts/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUser(string userId)
        {
            var posts = await _postRepository.GetByUserIdAsync(userId);
            return Ok(posts);
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            var result = await _postRepository.CreateAsync(post);

            if (!result)
            {
                return BadRequest("Failed to create post");
            }

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        // PUT: api/Posts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest("Post ID mismatch");
            }

            var existingPost = await _postRepository.GetByIdAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            var result = await _postRepository.UpdateAsync(id, post);
            if (!result)
            {
                return StatusCode(500, "Failed to update post");
            }

            return NoContent();
        }

        // DELETE: api/Posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var result = await _postRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete post");
            }

            return NoContent();
        }

        // POST: api/Posts/{id}/comments
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(string id, PostComment comment)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var result = await _postRepository.AddCommentAsync(id, comment);
            if (!result)
            {
                return StatusCode(500, "Failed to add comment");
            }

            return Ok();
        }

        // DELETE: api/Posts/{id}/comments/{userId}/{createdAt}
        [HttpDelete("{id}/comments/{userId}/{createdAt}")]
        public async Task<IActionResult> RemoveComment(string id, string userId, string createdAt)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var result = await _postRepository.RemoveCommentAsync(id, userId, createdAt);
            if (!result)
            {
                return StatusCode(500, "Failed to remove comment");
            }

            return Ok();
        }

        // POST: api/Posts/{id}/likes/{userId}
        [HttpPost("{id}/likes/{userId}")]
        public async Task<IActionResult> AddLike(string id, string userId)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var result = await _postRepository.AddLikeAsync(id, userId);
            if (!result)
            {
                return StatusCode(500, "Failed to add like");
            }

            return Ok();
        }

        // DELETE: api/Posts/{id}/likes/{userId}
        [HttpDelete("{id}/likes/{userId}")]
        public async Task<IActionResult> RemoveLike(string id, string userId)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var result = await _postRepository.RemoveLikeAsync(id, userId);
            if (!result)
            {
                return StatusCode(500, "Failed to remove like");
            }

            return Ok();
        }

        // POST: api/Posts/tags
        [HttpPost("tags")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByTags([FromBody] List<string> tags)
        {
            var posts = await _postRepository.GetByTagsAsync(tags);
            return Ok(posts);
        }
    }
}