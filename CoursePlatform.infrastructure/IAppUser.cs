using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.infrastructure
{

    public static class AppUserFlagData
    {
        public const string OrgRegion = "区域";

        public const string OrgSchool = "学校";

        public const string OrgSection = "学段";

        public const string OrgGrade = "年级";

        public const string RoleStudent = "学生";

        public const string RoleTeacher = "教师";

        public const string OrgResearch = "教研组";
    }


    public interface IAppUser
    {


        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; }

        void SetSession(string session);


        public string Session { get; }

        public int UserId { get; }


        public string UserName { get; }


        public string Role { get; }


        public bool IsTreacher { get; }


        Task<int> ClassIdAsync();


        Task<UserPropVal> Get(string OrgType);

        
        Task<UserPropVal> GetRegion();


        Task<UserPropVal> GetSchool();

        
        public IList<UserPropVal> Roles { get; }


        public bool IsStudent { get; }

    }

}
