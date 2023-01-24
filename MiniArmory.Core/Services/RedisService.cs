using MiniArmory.Core.Services.Contracts;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace MiniArmory.Core.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer connection;
        private readonly IDatabase redis;

        public RedisService(IConnectionMultiplexer connection)
        {
            this.connection = connection;

            this.redis = this.connection.GetDatabase();
        }

        public async Task<T> RetrieveCache<T>(string key)
        {
            var cachedObj = await this.redis.StringGetAsync(key);

            if (cachedObj.IsNull)
            {
                return default!;
            }

            return JsonConvert.DeserializeObject<T>(cachedObj!)!;
        }

        public async Task SetCache<T>(string key, T value)
        {
            TimeSpan expiry = new TimeSpan(0, 1, 0);

            await this.redis.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);
        }
    }
}
