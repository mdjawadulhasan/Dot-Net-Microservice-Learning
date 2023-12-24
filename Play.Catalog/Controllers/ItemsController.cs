using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Entities;
using Play.Catalog.Repo;
using static Play.Catalog.DTOs.DTOs;

namespace Play.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepo itemRepo;
        public ItemsController(IItemRepo itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetASync()
        {
            var items = await itemRepo.GetAllAsync();

            return items.Select(it => it.AsDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemRepo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create(CreatedItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreateDate = DateTimeOffset.Now
            };

            await itemRepo.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatedItemDto updateItemDto)
        {
            var existingItem = await itemRepo.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemRepo.UpdateAsync(existingItem);

            return Ok(existingItem);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingItem = await itemRepo.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            await itemRepo.RemoveAsync(existingItem.Id);
            return NoContent();
        }
    };
}

