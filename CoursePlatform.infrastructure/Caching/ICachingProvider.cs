using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Infrastructure.Caching
{
    public interface ICachingProvider
    {

        ///// <summary>
        ///// 从缓存中获取一个对象
        ///// </summary>
        ///// <typeparam name="T">获取的对象</typeparam>
        ///// <param name="key">键</param>
        ///// <param name="func">如果缓存中不存在，就从数据源中获取</param>
        ///// <param name="expire">过期时间</param>
        ///// <param name="field"></param>
        ///// <returns></returns>
        //Task<T> Get<T>(string key, Func<Task<T>> func, string field, TimeSpan? expire = null);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<T> Get<T>(string key, Func<Task<T>> func, TimeSpan? expire = null);

        /// <summary>
        /// 从缓存中获取对象，不存在返回空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<T> Get<T>(string key) => Get<T>(key, null, null);


        /// <summary>
        /// 从缓存中获取对象，不存在返回空
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<object> GetAsync(string key, Type type);


        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IList<T>> GetAll<T>(string key);

        /// <summary>
        /// 移除一个缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<bool> Remove(string key, params string[] field);


        /// <summary>
        /// 从String Cache中移除对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Remove(string key);

        /// <summary>
        /// 设置缓存对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">要缓存的对象</param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, object obj);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> RPopAsync<T>(string key);



        Task<bool> HasKey(string key); 

                
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task LPushXAsync<T>(string key, T data);
        Task HSetAsync<T>(string key, string field, T data);
        Task<T> HGetAsync<T>(string key, string field);
        bool HSet<T>(string key, string field, T data);
        T HGet<T>(string key, string field);
        long HDel(string key, params string[] field);
    }
}
