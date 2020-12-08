using EasyCaching.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Infrastructure.Caching
{
    public class RedisQueue : IRedisQueue
    {


        private readonly ICachingProvider _provider;


        private const string DEFAULTKEY = "D4L_QUEUE_3";

        public RedisQueue(ICachingProvider provider)
        {
            _provider = provider;
        }

        public async Task<T> GetAsync<T>(string key ="")
        {

            if (String.IsNullOrEmpty(key))
            {
                key = DEFAULTKEY;
            }


            var has = await this._provider.HasKey(key);

            if (!has)
            {
                return default;
            }

            var data = await this._provider.RPopAsync<string>(key);

            if (String.IsNullOrEmpty(data))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(data);
                       
        }


        public Task SetAsync<T>(T data, string key = "")
        {
            if (String.IsNullOrEmpty(key))
            {
                key = DEFAULTKEY;
            }

            string bufStr = JsonConvert.SerializeObject(data);

            return this._provider.LPushXAsync(key, bufStr);
        }

        public async Task<Guid> GetGuid(string key = null)
        {
            key ??= DEFAULTKEY;

            var has = await this._provider.HasKey(key);

            if (!has)
            {
                return Guid.Empty;
            }
            return await this._provider.RPopAsync<Guid>(key);

        }
    }
}
