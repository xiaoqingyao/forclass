using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.DTOS
{
 

    /// <summary>
    /// 分享给教师数据
    /// </summary>
    public class CollabratorDTO
    {

        /// <summary>
        /// 要分享的学习设计ID
        /// </summary>
        [Required]
        public string CourseId { get; set; }



        /// <summary>
        /// 要分享的学校教师对象   班级 > 教师
        /// </summary>

        [Required]
        public IList<CollbratorObjDTO> GradeObjs { get; set; }



        /// <summary>
        /// 要分享的教研组对象... 教研组 > 角色 > 教师
        /// </summary>
        [Required]

        public IList<CollbratorObjDTO> CommunityObjs { get; set; }

    }






    /// <summary>
    /// 要分享的对象信息 OrgId -> id
    /// 
    /// </summary>
    /// <remarks>
    ///      *   "SharedObjs": [
    ///{
    ///  "rootId": "string", -- 根节点Id，教研组代表教研组的ID，学校时代表年级的ID
    ///  "rootName": "string",-- 根节点名称，学校代表教研组的名称，学校时代表年级的名称
    ///  "id": "string", -- 教师ID
    ///  "name": "string", -- 教师名称
    ///  "orgId": "string", -- 教研组时角色ID，学习时班级Id 
    ///  "orgName": "string" -- 教研组时角色名称，学习时班级名称
    ///}
    /// </remarks>
    public class CollabratorSchoolDTO
    {

        /// <summary>
        /// 对象ID
        /// </summary>
        [Required]
        public int ObjId { get; set; }


        /// <summary>
        /// 对象名称
        /// </summary>
        [Required]
        public string ObjName { get; set; }



        /// <summary>
        /// 班级或角色ID
        /// </summary>
        public int OrgId { get; set; }



        /// <summary>
        /// 班级或角色名称 
        /// </summary>
        public string OrgName { get; set; }


    }

    /// <summary>
    /// RootId -> OrgId -> id
    /// </summary>
    public class CollbratorObjDTO : CollabratorSchoolDTO
    {
        /// <summary>
        /// 根节点ID
        /// </summary>
        public int RootId { get; set; }


        /// <summary>
        /// 根节点名称
        /// </summary>
        public string RootName { get; set; }


    }
}
