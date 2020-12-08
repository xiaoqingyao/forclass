using CoursePlatform.Domain.Permission;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace CoursePlatform.Domain.VO
{
    public class CourseVO : CourseOperation
    {



        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 署名
        /// </summary>
        public string SignatureName { get; set; }

        public string SignatureId { get; set; }



        /// <summary>
        /// 封面地址
        /// </summary>
        public string CoverUrl { get; set; }


        /// <summary>
        /// 目录
        /// </summary>
        public string CatalogId { get; set; }

        public string CatalogName { get; set; }


        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }

        ///// <summary>
        ///// 目标
        ///// </summary>
        ////public string Goal { get; set; }




        /// <summary>
        /// 标签
        /// </summary>
        public TagVO Tag { get; set; }


        ///// <summary>
        ///// 引用的学程
        ///// </summary>
        //public IList<DsItemVO> DsItems { get; set; }


        ///// <summary>
        ///// 协作者ID
        ///// </summary>

        //public HashSet<int> CollabratorId { get; set; }


        public string SchoolName { get; set; }


        public int RegionCode { get; set; }



        /// <summary>
        /// 加入学习的人数
        /// </summary>
        public int LeanerCount { get; set; }

        public string CreationDate { get; set; }


        /// <summary>
        /// 是否加入学习
        /// </summary>
        public bool IsJoined { get; set; }


        public int Status { get; set; }



        /// <summary>
        /// 区域审核状态..
        /// </summary>
        public int RegionStatus { get; set; }



        public int CollbratorCount { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    public class DsItemVO
    {
        public Guid DsId {get;set;}

        public int OperatorId { get; set; }

        public bool IsOpen { get; set; }

        public string DsName { get; set; }

        public string OperatorName { get; set; } 

        public string Cover { get; set; }

        public bool IsShared { get; set; }


        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOriginal { get; set; }

    }
}
