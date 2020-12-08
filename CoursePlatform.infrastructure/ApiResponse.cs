using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.infrastructure
{
    public class ApiResponse
    {
        public int Code { get; set; }

        public string Message { get; set; }

    }

    /// <summary>
    /// 接口返回接口
    /// </summary>
    /// <typeparam name="T">返回的数据</typeparam>
    /// <remarks>
    /// <see cref="ApiResponse.Code"/> 小于0时表示接口执行有异常。
    /// <see cref="ApiResponse.Message"/> 表示异常的信息
    /// </remarks>
    public class ApiResponseAsync<T> : ApiResponse
    {
        public T Data { get; set; }
    }


    public class ApiRes
    {
        public static async Task<ApiResponseAsync<T>> OKAsync<T>(Task<T> data)
        {
            var rev = new ApiResponseAsync<T>
            {
                Data = await data
            };
            return rev;
        }

        public static ApiResponseAsync<T> OK<T>(T data)
        {

            var rev = new ApiResponseAsync<T>
            {
                Data = data
            };
            return rev;
        }

        public static async Task<ApiResponseAsync<bool>> Error(string msg, int code = -1)
        {

            await Task.CompletedTask;

            var rev = new ApiResponseAsync<bool>
            {
                Data = false,
                Code = code,
                Message = msg
            };

           return rev;
           
        }
    }

}
