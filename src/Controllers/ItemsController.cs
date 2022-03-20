using CacheService.Data;
using CacheService.Dto;
using CacheService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CacheService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepo _repository;

        public ItemsController(IItemRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var allItems = await _repository.GetAllItems();
            var result = new List<ItemDto>();
            
            if (allItems == null)
            {
                return Ok(result);
            }
            
            foreach (var item in allItems.ToList())
            {
                if (item != null)
                    result.Add(new ItemDto()
                    {
                        Id = item.GetId(),
                        Value = item.Value
                    });
            }

            return Ok(result);
        }

        [HttpGet("{id}", Name="GetItemById")]
        public async Task<ActionResult<ItemDto>> GetItemById(string id)
        {
            var item = await _repository.GetItemById(id);
            
            if (item != null)
            {
                return Ok(new ItemDto()
                {
                    Id = item.GetId(),
                    Value = item.Value
                });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(ItemToCreateDto? itemToCreate)
        {
            if (itemToCreate == null)
            {
                return BadRequest();
            }

            var newItem = await _repository.CreateItem(new Item()
            {
                Value = itemToCreate.Value
            });

            var id = newItem.GetId();

            return CreatedAtRoute(nameof(GetItemById), new {Id = id}, new ItemDto()
            {
                Id = id,
                Value = newItem.Value
            });
        }
        
        [HttpPut]
        public async Task<ActionResult<ItemDto>> UpdateItem(ItemToUpdateDto? itemToUpdate)
        {
            if (itemToUpdate == null)
            {
                return BadRequest();
            }

            await _repository.UpdateItem(new Item()
            {
                Id = itemToUpdate.Id,
                Value = itemToUpdate.Value
            });

            return this.NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemById(string id)
        {
            await _repository.DeleteItem(id);
            return this.NoContent();
        }
    }
}
