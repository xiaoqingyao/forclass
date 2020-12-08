using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.Entities
{


    /// <summary>
    /// 课程信息
    /// </summary>
    public class CourseEntity : EntityBase
    {

        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }





        /// <summary>
        /// 署名ID  如果大于零为教研组，反之为自签
        /// </summary>
        public int SignatureId { get; set; }


        [Column(TypeName = "nvarchar(200)")]
        public string SignatureName { get; set; }


        [Column(TypeName = "nvarchar(500)")]
        public string CoverUrl { get; set; }


        [Column(TypeName = "nvarchar(200)")]
        public string CatalogId { get; set; }


        [Column(TypeName = "nvarchar(300)")]
        public string CatalogName { get; set; }

        public string Intro { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public string Goal { get; set; }


        [IndexColumn]
        public int Status { get; set; }


       [IndexColumn]
        public int RegionStatus { get; set; }



        /// <summary>
        /// 区域
        /// </summary>
        public int RegionCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string RegionName { get; set; }



        /// <summary>
        /// 学校
        /// </summary>
        public int SchoolCode { get; set; }


        
        [Column(TypeName = "nvarchar(100)")]
        public string SchoolName { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        [IndexColumn]
        public int CreatorCode { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string CreatorName { get; set; }



        [IndexColumn]
        public int CollbratorCount { get; set; }



        public int LeanerCount { get; set; }


        /// <summary>
        /// 学段
        /// </summary>
        public int Section { get; set; }

        /// <summary>
        ///  教研组Id
        /// </summary>

        public int ResearchGroupId { get; set; }

        /// <summary>
        /// 教研组名称
        /// </summary>

        [Column(TypeName = "nvarchar(100)")]
        public string ResearchGroupName { get; set; }


        /// <summary>
        /// 引用的学程数 
        /// </summary>
        public int CourseCount { get; set; }
    }
}
