using CacheService.Models;

namespace CacheService.Data
{
    public interface IBasketRepo
    {
        string Set(string value);
        Item? Get(string id);
    }
}