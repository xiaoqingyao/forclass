using CoursePlatform.Data.Entities;
using CoursePlatform.infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseCreated : ICoursePlatformEvent
    {


        public string Id { get; set; }


        public string Name { get; set; }



        /// <summary>
        /// 署名ID  如果大于零为教研组，反之为自签
        /// </summary>
        public int SignatureId { get; set; }


        public string SignatureName { get; set; }


        public string CoverUrl { get; set; }


        public string CatalogId { get; set; }


        public string CatalogName { get; set; }

        public string Intro { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public string Goal { get; set; }

        public int Status { get; set; }

        public int RegionStatus { get; set; }

        public TagEventVal Tag { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public UserPropVal Region { get; set; }



        /// <summary>
        /// 学校
        /// </summary>
        public UserPropVal School { get; set; }



        /// <summary>
        /// 创建人
        /// </summary>
        public UserPropVal Creator { get; set; }
        public string SchoolName { get; set; }
        public string RegionName { get; set; }


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

        public string ResearchGroupName { get; set; }

    }



    /// <summary>
    /// 课程署名
    /// </summary>
    public class TagEventVal
    {


        /// <summary>
        /// 如果Id > 0 教研组, 反之 自签
        /// </summary>
        public int Id { get; set; }


        public string Name { get; set; }


        public IList<TagEventItem> Items { get; set; }
    }


    public class TagEventItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }
    }



}
