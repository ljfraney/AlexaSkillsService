using AlexaSkillsService.Interfaces;
using RedisCacheManager;
using System;

namespace AlexaSkillsService.Utilities
{
    public class CacheAdapter : ICacheAdapter
    {
        private readonly IRedisManager _redisManager;

        public CacheAdapter(IRedisManager redisManager)
        {
            _redisManager = redisManager;
        }

        public void Add<T>(string key, T value, int minutesToExpire) where T : class
        {
            var cacheExpiry = DateTime.Now.AddMinutes(minutesToExpire);
            _redisManager.Set(key, value, cacheExpiry);
        }

        public T Get<T>(string key) where T : class
        {
            return _redisManager.Get<T>(key);
        }
    }
}