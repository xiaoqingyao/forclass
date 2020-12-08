using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.DTOS.Queries
{
    public class CourseParamters : PagnationDTO, IFilterParam
    {

        ///// <summary>
        ///// 是否学习
        ///// 如果为True表示加入学习的查询
        ///// </summary>
        //public bool IsLearning { get; set; }



        /// <summary>
        /// 状态
        /// 1：草稿
        /// 2: 校审核
        /// 3： 全部 = 草稿|校审核  = 1|2
        /// 16: 校上架
        /// </summary>
        //[Range(0,int.MaxValue)]
        public int Status { get; set; }

        //筛选的标签
        public IList<TagParam> Tags { get; set; }


        /// <summary>
        /// 开始日期 格式:yyyy-MM-dd
        /// </summary>
        public string StartDate { get; set; }


        /// <summary>
        /// 结束日期 格式：yyyy-MM-dd
        /// </summary>
        public string EndDate { get; set; }


        /// <summary>
        /// 查询类型: t: 老师， s: 学用， a: 审核用
        /// </summary>
        [Required]
        public char QueryType { get; set; }



        /// <summary>
        /// 组织Id
        /// </summary>
        public int OrgId { get; set; }


        /// <summary>
        /// 组织类型 s:学校，r:区域
        /// </summary>
        public char OrgType { get; set; }


        public char OrderBy { get; set; }

        public string Keyword { get; set; }

    }




    public record TagParam(string Name, string TypeName)
    {

    }

 }
