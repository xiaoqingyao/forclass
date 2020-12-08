using EasyCaching.Core;
using Microsoft.Extensions.Logging;
using CoursePlatform.Infrastructure.Serializer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CoursePlatform.Infrastructure.Caching
{
    public class CacheProvider : ICachingProvider
    {

        private readonly IEasyCachingProviderFactory _cachingProvider;
        private readonly IRedisCachingProvider _provider;
        private readonly ISerializer _serializer;
        //private readonly ILogger<CacheProvider> _looger;

        public CacheProvider(IEasyCachingProviderFactory easyCachingProviderFactory, ISerializer serializer/*, ILogger<CacheProvider> logger*/)
        {
            this._cachingProvider = easyCachingProviderFactory;
            this._provider = this._cachingProvider.GetRedisProvider("redis");
            this._serializer = serializer;
            //this._looger = logger;
        }





        /// <summary>
        ///  using redis StringSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key, Func<Task<T>> func, TimeSpan? expire = null)
        {




            string rev = await this._provider.StringGetAsync(key);

            if (String.IsNullOrEmpty(rev) && func != null)
            {

                var source = await func();

                if (source != null)
                {
                    string data = this._serializer.Serial(source);
                    await this._provider.StringSetAsync(key, data, expire);
                    //if (expire.HasValue)
                    //{
                    //    await this._provider.KeyExpireAsync(key, expire.Value.Seconds);
                    //}
                }
                return source;

            }

            if (String.IsNullOrEmpty(rev))
            {
                return default;
            }

            return this._serializer.Deserial<T>(rev);
        }




        ///// <summary>
        ///// using redis HSet...
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="func"></param>
        ///// <param name="field"></param>
        ///// <param name="expire"></param>
        ///// <returns></returns>
        //public async Task<T> Get<T>(string key, Func<Task<T>> func, string field, TimeSpan? expire = null)
        //{


        //    string rev = await this._provider.HGetAsync(key, field);

        //    if (String.IsNullOrEmpty(rev) && func != null)
        //    {

        //        var source = await func();

        //        if (source != null)
        //        {
        //            string data = this._serializer.Serial(source);
        //            await this._provider.HSetAsync(key, field, data);
        //            if (expire.HasValue)
        //            {
        //                await this._provider.KeyExpireAsync(key, expire.Value.Seconds);
        //            }
        //        }
        //        return source;

        //    }

        //    if (String.IsNullOrEmpty(rev))
        //    {
        //        return default;
        //    }

        //    return this._serializer.Deserial<T>(rev);

        //}


        /// <summary>
        /// <inheritdoc/> using redis string get...
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(string key, Type type)
        {
            var source = await this._provider.StringGetAsync(key);
            if (String.IsNullOrEmpty(source))
            {
                return null;
            }
            return this._serializer.Deserial(source, type);
        }


        /// <summary>
        /// <inheritdoc/> using redis hget...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetAll<T>(string key)
        {
            var cachedata = await this._provider.HGetAllAsync(key);
            if (cachedata == null || cachedata.Keys.Count == 0)
            {
                return null;
            }

            var revList = new List<T>();


            foreach (var item in cachedata.Keys)
            {

                T itemT = this._serializer.Deserial<T>(cachedata[item]);

                revList.Add(itemT);

            }

            return revList;
        }




        /// <inheritdoc/>
        public async Task<bool> Remove(string key)
        {
            await this._provider.KeyDelAsync(key);
            return true;
            //throw new NotImplementedException();

        }


        /// <inheritdoc/>
        public async Task<bool> Remove(string key, params string[] field)
        {
            if (field.Length == 0)
            {
                field = new[] { key };
            }

            await this._provider.HDelAsync(key, field);

            return true;
            //throw new NotImplementedException();

        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Task<bool> SetAsync(string key, object obj)
        {
            return this._provider.StringSetAsync(key, this._serializer.Serial(obj));
        }

        public Task<T> RPopAsync<T>(string key)
        {

            if (!this._provider.KeyExists(key))
            {
                return default;
            }

            return this._provider.RPopAsync<T>(key);
        }

        public Task LPushXAsync<T>(string key, T data)
        {
            this._provider.LPushX(key, data);
            return Task.CompletedTask;
        }

        public Task HSetAsync<T>(string key, string field, T data)
        {
            string json = this._serializer.Serial(data);

            return this._provider.HSetAsync(key, field, json);
        }

        public async Task<T> HGetAsync<T>(string key, string field)
        {
            string ret = await this._provider.HGetAsync(key, field);
            if (ret is null or {Length: <= 0 })
            {
                return default;
            }
            return this._serializer.Deserial<T>(ret);
        }


        public bool HSet<T>(string key, string field, T data)
        {
            string json = this._serializer.Serial(data);


           

            return this._provider.HSet(key, field, json);
        }

        public long HDel(string key, params string[] field)
        {
            return this._provider.HDel(key, field);
        }

        public T HGet<T>(string key, string field)
        {
            string ret = this._provider.HGet(key, field);
            if (ret is null or { Length: <= 0 })
            {
                return default;
            }
            return this._serializer.Deserial<T>(ret);
        }


        public Task<bool> HasKey(string key)
        {
            return this._provider.KeyExistsAsync(key);
        }
    }
}
