using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
//using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
//using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace CoursePlatform.Data.Entities
{
    public class CollabratorEntity : EntityBase
    {
        [IndexColumn]

        public int ObjId { get; set; }

        [IndexColumn]
        public string CourseId { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string ObjName { get; set; }


        /// <summary>
        /// 根Id
        /// </summary>

        public int RootId { get; set; }



        /// <summary>
        /// 根名称
        /// </summary>

        [Column(TypeName = "nvarchar(50)")]
        public string RootName { get; set; }


        /// <summary>
        /// 组织Id，班级或角色
        /// </summary>

        public int OrgId { get; set; }


        /// <summary>
        /// 组织名称
        /// </summary>

        [Column(TypeName = "nvarchar(50)")]
        public string OrgName { get; set; }


        /// <summary>
        /// 分享类型
        /// </summary>

        [Column(TypeName = "nvarchar(50)")]
        public int Type { get; set; }
    }

}
