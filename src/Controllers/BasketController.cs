using CacheService.Data;
using CacheService.Dto;
using CacheService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CacheService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepo _repository;

        public BasketController(IBasketRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}", Name="GetItem")]
        public async Task<ActionResult<ItemDto>> GetItem(string id)
        {
            var item = await _repository.Get(id);
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
        public async Task<ActionResult<Item>> SetItem(ItemToCreateDto? itemToCreate)
        {
            if (itemToCreate == null)
            {
                return BadRequest();
            }

            var id = await _repository.Set(itemToCreate.Value);
            return CreatedAtRoute(nameof(GetItem), new {Id = id}, 
                new ItemDto
                {
                    Id = id,
                    Value = itemToCreate.Value
                });
        }
    }
}
