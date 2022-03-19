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

        DELETE
            UPDATE (replce)
        ASYNC
        
        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            var allItems = _repository.GetAllItems();
            
            if (allItems == null) return NotFound();
            
            var result = new List<ItemDto>();
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
        public ActionResult<ItemDto> GetItemById(string id)
        {
            var item = _repository.GetItemById(id);
            
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
        public ActionResult<ItemDto> CreateItem(ItemToCreateDto? itemToCreate)
        {
            if (itemToCreate == null)
            {
                return BadRequest();
            }

            var newItem = _repository.CreateItem(new Item()
            {
                Value = itemToCreate.Value
            });

            return CreatedAtRoute(nameof(GetItemById), new {Id = newItem.GetId()}, new ItemDto()
            {
                Id = newItem.GetId(),
                Value = newItem.Value
            });
        }
    }
}
