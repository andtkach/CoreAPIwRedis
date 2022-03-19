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
        public ActionResult<ItemDto> GetItem(string id)
        {
            var item = _repository.Get(id);
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
        public ActionResult<Item> SetItem(ItemToCreateDto? itemToCreate)
        {
            if (itemToCreate == null)
            {
                return BadRequest();
            }

            var id = _repository.Set(itemToCreate.Value);
            return CreatedAtRoute(nameof(GetItem), new {Id = id}, 
                new ItemDto
                {
                    Id = id,
                    Value = itemToCreate.Value
                });
        }
    }
}
