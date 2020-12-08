using System;

namespace CoursePlatform.Asset
{
    public class ApiOptions
    {

        public const string SectionName = "ApiOptions";

        /// <summary>
        /// 用户服务接口I
        /// </summary>
        public string UserSessionUriPart { get; set; }


        /// <summary>
        /// 用户信息接口地址主机
        /// </summary>
        public string UserHostBase { get; set; }
        /// <summary>
        /// 用户组织架构
        /// </summary>
        public string UserOrganizationUriPart { get; set; }

        /// <summary>
        /// 获取学生
        /// </summary>
        public string UriPartGetStudents { get; set; }

        /// <summary>
        /// 用户角色接口
        /// </summary>
        public string UserRoleUrlPart { get; set; }

        /// <summary>
        /// 用户所属教研组接口地址
        /// </summary>
        public string CommunityUrlPart { get; set; }


        /// <summary>
        /// 获取教师列表
        /// </summary>
        public string TeacherListByOrg { get; set; }



        /// <summary>
        /// 教研组成员接口
        /// </summary>
        public string ComunityUserUrlPart { get; set; }


        /// <summary>
        /// 获取班级下教师信息....
        /// </summary>
        public string TeacherByClassUrlPart { get; set; }


        /// <summary>
        /// 获取教师-按班级分组
        /// </summary>
        public string TeacherGroupClassUrlPart { get; set; }


        /// <summary>
        /// 获取分组
        /// </summary>
        public string ClassGroupUrlPart { get; set; }


        /// <summary>
        /// 目录Api
        /// </summary>
        public string CatalogUrl { get; set; }

    }
}
