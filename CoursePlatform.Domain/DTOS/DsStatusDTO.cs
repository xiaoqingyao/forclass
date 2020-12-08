using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.DTOS
{
    public class DsStatusDTO
    {

        /// <summary>
        /// 课程ID
        /// </summary>
        [Required]
        public string CourseId { get; set; }


        /// <summary>
        /// 学程ID
        /// </summary>
        [Required]
        public Guid DsId { get; set; }


        /// <summary>
        /// 目录ID
        /// </summary>
        [Range(1,int.MaxValue)]
        public int CatalogId {get;set;}

        /// <summary>
        /// 状态
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
