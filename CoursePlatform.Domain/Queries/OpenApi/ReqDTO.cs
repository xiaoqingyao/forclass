using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.OpenApi
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class ReqDTO
    {


        /// <summary>
        /// 页大小
        /// </summary>
        [Range(1,100)]
        public int PageSize { get; set; } 



        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue)]

        public int PageNum { get; set; }



        /// <summary>
        /// 区域
        /// </summary>
        [Range(1, int.MaxValue)]
        public int OrgId { get; set; }



    }





}
