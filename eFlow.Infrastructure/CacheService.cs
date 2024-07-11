
using eFlow.Application.Interfaces.Caching;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace eFlow.Infrastructure
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;      
        private readonly IConfiguration _configuration;
        public CacheService(IConfiguration configuration)
        {           
            _configuration = configuration;
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
                return JsonSerializer.Deserialize<T>(value!);
            return default;
        }

        public async Task RemoveAsync(string key)
        {
            var _exist = await _cacheDb.KeyExistsAsync(key);
            if (_exist)
                await _cacheDb.KeyDeleteAsync(key);                      
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var expiryTime = TimeSpan.FromMinutes(2);
            await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        }
    }
}
