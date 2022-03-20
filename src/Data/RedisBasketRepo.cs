using System.Text.Json;
using CacheService.Models;
using StackExchange.Redis;

namespace CacheService.Data
{
    public class RedisBasketRepo : IBasketRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisBasketRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<string> Set(string value)
        {
            var item = new Item()
            {
                Value = value
            };
            
            if (item == null)
            {
                throw new ArgumentException($"{nameof(item)} is null");
            }
            
            var db = _redis.GetDatabase();
            var itemStr = JsonSerializer.Serialize(item);
            await db.StringSetAsync(item.Id, itemStr, TimeSpan.FromMinutes(1));
            return item.GetId();
        }

        public async Task<Item?> Get(string id)
        {
            var db = _redis.GetDatabase();

            var itemStr = await db.StringGetAsync($"{Item.Name}{Item.S}{id}");

            if (!string.IsNullOrEmpty(itemStr))
            {
                return JsonSerializer.Deserialize<Item>(itemStr);
            }
            
            return null;
        }
    }
}
