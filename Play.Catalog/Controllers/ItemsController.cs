using Microsoft.AspNetCore.Mvc;
using static Play.Catalog.DTOs.DTOs;

namespace Play.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static List<ItemDto> _items = new List<ItemDto>
        {
            new ItemDto(Guid.NewGuid(),"Item 1","Description 1",19.99m,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Item 2","Description 2",29.99m,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Item 3","Description 3",39.99m,DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public ActionResult<IEnumerable<ItemDto>> Get()
        {
            return _items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ItemDto> Create(CreatedItemDto createItemDto)
        {
            var newItem = new ItemDto(Id: Guid.NewGuid(),
                                      Name: createItemDto.Name,
                                      Description: createItemDto.Description,
                                      Price: createItemDto.Price,
                                      CreatedDate: DateTimeOffset.UtcNow);

            _items.Add(newItem);

            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, UpdatedItemDto updateItemDto)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price,
            };

            var index = _items.FindIndex(x => x.Id == id);
            _items[index] = updatedItem;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
            {
                return NotFound();
            }

            _items.Remove(existingItem);

            return NoContent();
        }
    };
}

