using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Infrastructure.Caching
{
    public interface IRedisQueue
    {


        /// <summary>
        ///  获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
       Task<T> GetAsync<T>(string key = null);


        Task<Guid> GetGuid(string key = null);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task SetAsync<T>(T data, string key = null);

    }
}
