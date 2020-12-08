using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using CoursePlatform.Asset.ApiDTO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursePlatform.infrastructure.Validators;
using CoursePlatform.infrastructure.Exceptions;

namespace CoursePlatform.Asset
{
    public class UserProxy : IUserProxy
    {

        private readonly IOptions<ApiOptions> _opt;

        public UserProxy(IOptions<ApiOptions> opt)
        {
            _opt = opt;
        }

        public async Task<OrgItem> GetOrg(string session)
        {
            var rev = await this.GetAsync<OrgDTO>(session, this._opt.Value.UserOrganizationUriPart);

            return rev.Result.FirstOrDefault();

        }

        public async Task<UserItem> GetUserAsync(string session)
        {

            //await Task.CompletedTask;

            //return new UserItem
            //{
            //    UserID = 1222,
            //    RealName = "hello"
            //};

            var rev = await this.GetAsync<UserInfoDTO>(session, this._opt.Value.UserSessionUriPart);

            var user = rev.Result.FirstOrDefault();

            Prosecutor.NotNull(user);


            return user;

        }

        public UserItem GetUser(string session)
        {

            //await Task.CompletedTask;

            //return new UserItem
            //{
            //    UserID = 1222,
            //    RealName = "hello"
            //};

            var rev = this.Get<UserInfoDTO>(session, this._opt.Value.UserSessionUriPart);

            var user = rev.Result.FirstOrDefault();

            Prosecutor.NotNull(user);


            return user;

        }



        public async Task<IList<RoleItem>> GetUserRoleAsync(string session)
        {

            var rev = await this.GetAsync<UserRoleDTO>(session, this._opt.Value.UserRoleUrlPart);

            return rev.result;
        }


        public IList<RoleItem> GetUserRole(string session)
        {

            var rev = this.Get<UserRoleDTO>(session, this._opt.Value.UserRoleUrlPart);

            return rev.result;
        }


        /// <summary>
        /// 获取学校按班级分组的教师
        /// </summary>
        /// <param name="session"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        public async Task<IList<TeacherGroupClassResult>> TeacherGroupClass(string session, int schoolId)
        {


            var rev = await this.GetAsync<TeacherGroupClassRoot>(session, this._opt.Value.TeacherGroupClassUrlPart, rq =>
            {
                rq.AddParameter("schoolidxs", schoolId);
            });

            return rev.result;

        }


        /// <summary>
        /// 获取用户教研组信息..
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task<IList<CommunityResult>> GetUserCommunity(string session)
        {
            var result = await this.GetAsync<UserCommunityRootobject>(session, this._opt.Value.CommunityUrlPart);
            if (result.result is null or { Count: <= 0 })
            {
                return null;
            }
            return result.result.Where(r => !String.IsNullOrEmpty(r.Idx)).ToList();

        }



        /// <summary>
        ///  教研组下所有的教师信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="comIds"></param>
        /// <returns></returns>
        public async Task<IList<CommunityMemberResult>> GetCommunityMember(string session, string comIds)
        {
            var rev = await this.GetAsync<CommunityMemberRootobject>(session,
                this._opt.Value.ComunityUserUrlPart,
                rq => rq.AddParameter("communityIdxs", comIds));
            return rev.result;
        }


        /// <summary>
        /// 获取该Session下用户所在教研级下的所有老师 
        ///</summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task<IList<CommunityMemberResult>> ComunityMemberByUser(string session)
        {
            var comunities = await this.GetUserCommunity(session);
            if (comunities is null or {Count: <= 0 })
            {
                return null;
            }
            var aryId = comunities.Where(c => !String.IsNullOrEmpty(c.Idx)).Select(s => s.Idx);
            string param = String.Join(",", aryId);

            var members = await this.GetCommunityMember(session, param);
            if (members is null or {Count:<=0 })
            {
                return null;
            }

            return members;
        }


        ///// <summary>
        ///// <inheritdoc/>
        ///// </summary>
        ///// <param name="session"></param>
        ///// <returns></returns>
        //public async Task<IList<TeacherGroupClassResult>> TeacherGroupClass(string session, int schoolId)
        //{

       
        //    var rev = await this.GetAsync<TeacherGroupClassRoot>(session, this._opt.Value.TeacherGroupClassUrlPart, rq =>
        //    {
        //        rq.AddParameter("schoolidxs", schoolId);
        //    });

        //    return rev.result;

        //}


        #region Internal methods.......

        private async Task<T> GetAsync<T>(string session, string urlPart, Action<IRestRequest> rqBuilder = null) where T : class, new()
        {

            IRestResponse rs = null;

            try
            {
                rs = await this.Client(urlPart).ExecuteAsync(this.Rq(session, rqBuilder));

                var rev = JsonConvert.DeserializeObject<T>(rs.Content);


                return rev;

            }
            catch (Exception ex)
            {

                throw new CP3PartApiException($"用户接口异常:{rs?.Content} -- {ex.Message}");
            }
        }


        private T Get<T>(string session, string urlPart, Action<IRestRequest> rqBuilder = null) where T : class, new()
        {

            IRestResponse rs = null;

            try
            {
                rs = this.Client(urlPart).Execute(this.Rq(session, rqBuilder));

                var rev = JsonConvert.DeserializeObject<T>(rs.Content);


                return rev;

            }
            catch (Exception ex)
            {

                throw new CP3PartApiException($"用户接口异常:{rs?.Content} -- {ex.Message}");
            }
        }


        private IRestRequest Rq(string session, Action<IRestRequest> rqBuild)
        {
            var ret = new RestRequest(Method.POST).AddParameter("session", session);
            rqBuild?.Invoke(ret);
            return ret;
        }

        private IRestClient Client(string urlPart)
        {
            return new RestClient(string.Concat(this._opt.Value.UserHostBase, urlPart));
        }

        #endregion

    }
}
