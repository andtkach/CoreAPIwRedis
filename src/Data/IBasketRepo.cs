using CacheService.Models;

namespace CacheService.Data
{
    public interface IBasketRepo
    {
        Task<string> Set(string value);
        Task<Item?> Get(string id);
    }
}