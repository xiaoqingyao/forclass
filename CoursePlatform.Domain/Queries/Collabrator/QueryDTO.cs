using CoursePlatform.Domain.Queries.Share;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoursePlatform.Domain.Queries.Collabrator
{
    public class QueryDTO
    {


        /// <summary>
        /// 要协作的课程ID
        /// </summary>
        [Required]
        public string CourseId { get; set; }



        /// <summary>
        /// 可协作的对象1:学校, 2:教研组
        /// </summary>
        [Required]
        public CollabratorType Type { get; set; }

    }
}
