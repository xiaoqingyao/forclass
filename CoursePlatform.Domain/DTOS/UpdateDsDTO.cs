using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.DTOS
{
    public class UpdateDsDTO
    {

        /// <summary>
        /// 课程ID
        /// </summary>
        [Required]
        public string CourseId { get; set; }



        /// <summary>
        /// 目录ID
        /// </summary>
        [Required]
        public int CatalogId { get; set; }


        /// <summary>
        /// 要修改学程信息
        /// </summary>
        public QuoteDSDTOItem Item { get; set; }
    }
}
