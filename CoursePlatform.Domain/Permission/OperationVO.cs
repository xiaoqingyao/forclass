using CoursePlatform.infrastructure.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoursePlatform.Domain.Permission
{

    public abstract class CourseOperation
    {
        /// <summary>
        /// 当前用户可执行的校审核操作
        /// </summary>
        [JsonIgnore]
        public IList<OperationVO> SchoolOperation { get; set; }

        /// <summary>
        /// 当前用户可执行的区域操作
        /// </summary>
        [JsonIgnore]
        public IList<OperationVO> RegionOperation { get; set; }


        /// <summary>
        /// 当前用户执行的个人操作
        /// </summary>
        [JsonIgnore]
        public IList<OperationVO> PersonalOperation { get; set; }


        /// <summary>
        /// 全部操作
        /// </summary>
        public IList<OperationVO> AllOperations
        {
            get;set;     
        }

        public bool EnableEdit
        {
            get;set;
        }

        /// <summary>
        /// 是否是创建者
        /// </summary>
        public bool IsCreator { get; set; }

        /// <summary>
        /// 是否是协作者
        /// </summary>
        public bool IsCollabrator { get; set; }

    }


    public class OperationVO
    {
        public string Text { get; set; }

        public string URL { get; set; }
    }
}
