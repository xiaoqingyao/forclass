using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.Entities
{
    public class PlatformUserJoinedCourseEntity : EntityBase
    {

        [IndexColumn]
        [Column(TypeName = "nvarchar(100)")]
        public string PlatUserId { get; set; }


        [IndexColumn]
        public int UserId { get; set; }

        [IndexColumn]
        [Column(TypeName = "nvarchar(100)")]
        public string CourseId { get; set; }


       public int GroupId { get; set; }



        public string GroupName { get; set; }


        [Column]
        public int JoinType { get; set; }

        ///// <summary>
        ///// 创建者删除
        ///// </summary>
        //[IndexColumn]
        //public bool CreatorDelete { get; set; }

    }
}
