using CacheService.Models;

namespace CacheService.Data
{
    public interface IItemRepo
    {
        Task<Item> CreateItem(Item item);
        Task<Item?> GetItemById(string id);
        Task<IEnumerable<Item?>?> GetAllItems();
        Task<Item?> UpdateItem(Item item);
        Task DeleteItem(string id);
    }
}