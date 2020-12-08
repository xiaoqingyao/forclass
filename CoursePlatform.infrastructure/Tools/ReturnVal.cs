using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure.Tools
{

    /// <summary>
    /// 数据返回统一格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReturnVal<T>
    {


        /// <summary>
        /// 当返回结果长度超过浏览器限制时，做数据分页使用
        /// </summary>
        public int NextPage { get; set; }


        /// <summary>
        /// 状态码,0 表示成功
        /// </summary>
        public int ReturnCode { get; set; }


        /// <summary>
        /// 状态文本
        /// </summary>
        public string ReturnText { get; set; }


        /// <summary>
        /// 具体数据
        /// </summary>
        public IList<T> Result { get; set; }
    }
}
