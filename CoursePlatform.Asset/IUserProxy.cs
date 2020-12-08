using CoursePlatform.Asset.ApiDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Asset
{
    public interface IUserProxy
    {

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task<UserItem> GetUserAsync(string session);



        /// <summary>
        /// 获取用户组织结构...
        /// </summary>
        /// <returns></returns>
        Task<OrgItem> GetOrg(string session);


        UserItem GetUser(string session);
        Task<IList<RoleItem>> GetUserRoleAsync(string session);
        IList<RoleItem> GetUserRole(string session);
        Task<IList<CommunityMemberResult>> ComunityMemberByUser(string session);
        Task<IList<CommunityMemberResult>> GetCommunityMember(string session, string comIds);
        Task<IList<TeacherGroupClassResult>> TeacherGroupClass(string session, int schoolId);
    }
}
