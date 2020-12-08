using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.DTOS
{

    /// <summary>
    /// 创建课程参数
    /// </summary>
    public class CreateCourseDTO
    {


        /// <summary>
        /// 空为创建 反之修改
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 课程名称
        /// </summary>
        
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// 署名ID  如果大于零为教研组，反之为自签
        /// </summary>
        public int SignatureId { get; set; }


        /// <summary>
        /// 署名
        /// </summary>
        public string SignatureName { get; set; }



        /// <summary>
        /// 封面路径
        /// </summary>
        public string CoverUrl { get; set; }


        /// <summary>
        /// 目录Id结构  1/1/1/1/1
        /// </summary>
        public string CatalogId { get; set; }


        /// <summary>
        /// 目录名称结构 目录/目录/目录
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Intro { get; set; }

        ///// <summary>
        ///// 目标
        ///// </summary>
        //public string Goal { get; set; }



        /// <summary>
        /// 标签
        /// </summary>
        public Tag Tag { get; set; }
    }


    /// <summary>
    /// 标签
    /// </summary>
    public class Tag
    {


        /// <summary>
        ///  标签属性ID
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 标签属性名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 标签数组
        /// </summary>
        public IList<TagDTOItem> Items { get; set; }
    }


    /// <summary>
    /// 单个标签属性
    /// </summary>
    public class TagDTOItem
    {

        /// <summary>
        /// 标签ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 标签所属类型名称： 版本、学科、年级 etc.........
        /// </summary>
        public string TypeName { get; set; }
    }
}
