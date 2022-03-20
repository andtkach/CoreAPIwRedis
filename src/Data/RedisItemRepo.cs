using System.Text.Json;
using CacheService.Models;
using StackExchange.Redis;

namespace CacheService.Data
{
    public class RedisItemRepo : IItemRepo
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly string _nameSet = "Items";

        public RedisItemRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<Item> CreateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentException($"{nameof(item)} is null");
            }

            var db = _redis.GetDatabase();

            var itemStr = JsonSerializer.Serialize(item);

            await db.HashSetAsync(this._nameSet, new HashEntry[]
            {
                new HashEntry(item.Id, itemStr)
            });

            return item;
        }

        public async Task<Item?> GetItemById(string id)
        {
            var db = _redis.GetDatabase();

            var itemStr = await db.HashGetAsync(this._nameSet, $"{Item.Name}{Item.S}{id}");

            if (!string.IsNullOrEmpty(itemStr))
            {
                return JsonSerializer.Deserialize<Item>(itemStr);
            }
            
            return null;
        }

        public async Task<IEnumerable<Item?>?> GetAllItems()
        {
            var db = _redis.GetDatabase();

            var completeSet = await db.HashGetAllAsync(this._nameSet);
            
            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => 
                    JsonSerializer.Deserialize<Item>(val.Value)).ToList();
                return obj;   
            }
            
            return null;
        }

        public async Task<Item?> UpdateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentException($"{nameof(item)} is null");
            }
            
            var db = _redis.GetDatabase();
            item.Id = $"{Item.Name}{Item.S}{item.Id}";
            
            var existingItemStr = await db.HashGetAsync(this._nameSet, item.Id);

            if (string.IsNullOrEmpty(existingItemStr))
            {
                return null;
            }
            
            await db.HashDeleteAsync(this._nameSet, item.Id);
            var itemStr = JsonSerializer.Serialize(item);
            await db.HashSetAsync(this._nameSet, new HashEntry[]
            {
                new HashEntry(item.Id, itemStr)
            });

            return item;
        }

        public async Task DeleteItem(string id)
        {
            var db = _redis.GetDatabase();
            await db.HashDeleteAsync(this._nameSet, $"{Item.Name}{Item.S}{id}");
        }
    }
}
