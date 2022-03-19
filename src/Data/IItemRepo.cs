using CacheService.Models;

namespace CacheService.Data
{
    public interface IItemRepo
    {
        Item CreateItem(Item item);
        Item? GetItemById(string id);
        IEnumerable<Item?>? GetAllItems();
    }
}