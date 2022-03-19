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

        public Item CreateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentException($"{nameof(item)} is null");
            }

            var db = _redis.GetDatabase();

            var itemStr = JsonSerializer.Serialize(item);

            db.HashSet(this._nameSet, new HashEntry[]
            {
                new HashEntry(item.Id, itemStr)
            });

            return item;
        }

        public Item? GetItemById(string id)
        {
            var db = _redis.GetDatabase();

            var itemStr = db.HashGet(this._nameSet, $"{Item.Name}{Item.S}{id}");

            if (!string.IsNullOrEmpty(itemStr))
            {
                return JsonSerializer.Deserialize<Item>(itemStr);
            }
            
            return null;
        }

        public IEnumerable<Item?>? GetAllItems()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.HashGetAll(this._nameSet);
            
            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => 
                    JsonSerializer.Deserialize<Item>(val.Value)).ToList();
                return obj;   
            }
            
            return null;
        }
    }
}
