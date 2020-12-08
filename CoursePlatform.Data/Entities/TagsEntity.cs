using CoursePlatform.Data.EFProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Text;

namespace CoursePlatform.Data.Entities
{


    /// <summary>
    /// 标签及标签分类
    /// </summary>
    public class TagsEntity : EntityBase
    {


        /// <summary>
        /// 对应课程ID
        /// </summary>

        [Column(TypeName = "nvarchar(50)")]
        public string CourseId { get; set; }

        
        /// <summary>
        /// 学校Id
        /// </summary>
        public int SchoolId { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string SchoolName { get; set; }


        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegtionId { get; set; }


        [Column(TypeName = "nvarchar(100)")]
        public string RegionName { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>

        public int Creator { get; set; }



        /// <summary>
        /// 对应资源Id
        /// </summary>
        
        public int AssetId { get; set; }



        /// <summary>
        /// 标签名称
        /// </summary>
        
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }


        /// <summary>
        /// 标签的分类Id, ID == 0 为标签分类数据
        /// </summary>
        public int CategoryId { get; set; }


        /// <summary>
        /// 标签属性名
        /// </summary>
        
        [Column(TypeName = "nvarchar(50)")]
        public string TypeName { get; set; }


        /// <summary>
        /// 对应的课堂状态
        /// </summary>
        public int Status { get; set; }


    }
}
