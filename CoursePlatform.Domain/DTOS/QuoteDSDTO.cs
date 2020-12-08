using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace CoursePlatform.Domain.DTOS
{
    public class QuoteDSDTO
    {

        /// <summary>
        /// 所属目录ID
        /// </summary>
        [Range(1, int.MaxValue)]
        public int CatalogId { get; set; }         


        /// <summary>
        /// 课程ID
        /// </summary>
        [Required]
        public string CourseId { get; set; }

        /// <summary>
        /// 所含学程
        /// </summary>
        [Required]
        public IList<QuoteDSDTOItem> Items { get; set; }


    }


    public class QuoteDSDTOItem
    {



        /// <summary>
        /// 引用的学程ID
        /// </summary>
        [Required]
        public Guid DsId { get; set; }



        /// <summary>
        /// 学程名称
        /// </summary>
        [Required]
        public string DsName { get; set; }




        /// <summary>
        /// 学程封面
        /// </summary>
        public string Cover { get; set; }


        /// <summary>
        /// 是否公开
        /// </summary>

        public bool IsOpen { get; set; }


        /// <summary>
        /// 是否已共享
        /// </summary>
        public bool IsShared { get; set; }


        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOriginal { get; set; }



    }

}
