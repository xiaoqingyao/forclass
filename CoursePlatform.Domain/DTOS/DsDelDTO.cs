using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.DTOS
{
    public class DsDelDTO
    {

        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId {get;set;}


        /// <summary>
        /// 目录ID
        /// </summary>
        public int CatalogId { get; set; }


        /// <summary>
        /// 学程ID
        /// </summary>
        public Guid DsId { get; set; }
    }
}
