using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await _itemRepository.GetAllAsync();
            return Ok(items);
        }

        // GET: api/Items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // GET: api/Items/type/{type}
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByType(string type)
        {
            var items = await _itemRepository.GetByTypeAsync(type);
            return Ok(items);
        }

        // GET: api/Items/rarity/{rarity}
        [HttpGet("rarity/{rarity}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByRarity(string rarity)
        {
            var items = await _itemRepository.GetByRarityAsync(rarity);
            return Ok(items);
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            var result = await _itemRepository.CreateAsync(item);

            if (!result)
            {
                return BadRequest("Failed to create item");
            }

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // PUT: api/Items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(string id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest("Item ID mismatch");
            }

            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            var result = await _itemRepository.UpdateAsync(id, item);
            if (!result)
            {
                return StatusCode(500, "Failed to update item");
            }

            return NoContent();
        }

        // DELETE: api/Items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var result = await _itemRepository.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "Failed to delete item");
            }

            return NoContent();
        }
    }
}