using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.Entities
{
    public class PlatformUserEntity : EntityBase
    {



        /// <summary>
        /// 用户Id
        /// </summary>
        [IndexColumn]
        public int UserId { get; set; }


        /// <summary>
        /// 所属学校ID
        /// </summary>
        public int SchoolId { get; set; }


        /// <summary>
        /// 区域Id
        /// </summary>
        public int SectionId { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }


        /// <summary>
        /// 已上架的商品
        /// </summary>
        public int CourseShelves { get; set; }


        /// <summary>
        /// 加入学习的学生数
        /// </summary>
        public int StdJoined { get; set; }


        /// <summary>
        /// 创建的课程数
        /// </summary>
        public int CourseCount { get; set; }


        //public int ResearchGroupId { get; set; }



        //[Column(TypeName = "nvarchar(100)")]
        //public string ResearchGroupName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string SchoolName { get; set; }
    }
}
