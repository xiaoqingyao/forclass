using CoursePlatform.Domain.Permission;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.VO
{
    public class CoursePagingVO : CourseOperation
    {



        //public int RegionCode { get; set; }

        //public int SchoolCode { get; set; }

        //public int CreatorCode { get; set; }


        /// <summary>
        /// 课程ID
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }


        /// <summary>
        /// 署名
        /// </summary>
        public string SignatureName { get; set; }


        /// <summary>
        /// 创建者学校Id
        /// </summary>
        public string SchoolName { get; set; }


        /// <summary>
        /// 封面
        /// </summary>
        public string CoverUrl { get; set; }


        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 区域审核状态..
        /// </summary>
        public int RegionStatus { get; set; }


        /// <summary>
        /// 加入学习的人数
        /// </summary>
        public int LearnerCount { get; set; }


        public int CollbratorCount { get; set; }


        public int CreatorCode { get; set; }


        public string CreatorName { get; set; }


        [JsonIgnore]
        public string Intro { get; set; }

        [JsonIgnore]
        public string CatalogName { get; set; }

        [JsonIgnore]
        public string CatalogId { get; set; }

        public DateTime? CreationTime { get; set; }// = DateTime.Now;
    }
}
