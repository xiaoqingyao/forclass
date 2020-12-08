using LinqToDB.DataProvider.SapHana;
using LinqToDB.Tools;
using CoursePlatform.Asset;
using CoursePlatform.Asset.ApiDTO;
using CoursePlatform.infrastructure;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CoursePlatform.Application.Modules
{
    public class AppUser : IAppUser
    {


        private readonly ICoursePlatformHttpContext _ctx;
        private readonly IUserProxy _userSvc;



        public AppUser(ICoursePlatformHttpContext ctx, IUserProxy userSvc)
        {

            _ctx = ctx;
            _userSvc = userSvc;
        }

        private string _session;

        public string Session
        {
            get
            {
                if (String.IsNullOrEmpty(_session))
                {
                    this._session = this._ctx.Session;
                }
                return _session; //this._ctx.Session;
            }
        }//throw new NotImplementedException();







        private UserItem user;

        public int UserId
        {
            get
            {

                if (!this.IsLogin)
                {
                    return 0;
                }

                if (user == null)
                {
                    this.user = _userSvc.GetUser(this.Session);//.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                return user.UserID;
            }
        }



        public string UserName
        {
            get
            {
                if (user == null)
                {
                    this.user = _userSvc.GetUser(this.Session);//.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                return user.RealName;
            }
        }



        public string Role => throw new NotImplementedException();

        public bool IsTreacher
        {
            get
            {
                return this.GetRole(AppUserFlagData.RoleTeacher) is not null;
                
            }
        }



        public UserPropVal GetRole(string name)
        {
            if (this.Session is null or { Length: <= 0 })
            {
                return null;
            }
            if (this.Roles.Count is 0)
            {
                return null;
            }
            return this.Roles.FirstOrDefault(r => r.Name == name);
        }  

        public void SetSession(string session)
        {


        }

        public IDictionary<string, UserPropVal> Org { get; set; }


        public UserPropVal Region { get; set; }

        public IList<UserPropVal> Schools { get; set; }


        public IList<UserPropVal> Section { get; set; }


        public IList<UserPropVal> Grade { get; set; }

        public IList<UserPropVal> Class { get; set; }


        private IList<UserPropVal> _roles;

        public IList<UserPropVal> Roles
        {
            get
            {

                if (_roles != null)
                {
                    return this._roles;
                }

                var ret = this._userSvc.GetUserRole(this.Session);

                if (ret == null || ret.Count == 0)
                {
                    return null;
                }

                this._roles = ret.Select(r => new UserPropVal
                {
                    Code = r.Idx,
                    Name = r.Name
                }).ToList();

                return this._roles;
            }
        }

        public bool IsLogin => this.Session is not null and { Length: > 0 };

        public bool IsStudent => this.GetRole(AppUserFlagData.RoleStudent) is not null; //throw new NotImplementedException();

        public IList<UserPropVal> AddOrgItem(IList<UserPropVal> source, OrgItem item, string parentCode, int deep)
        {
            if (source == null)
            {
                source = new List<UserPropVal>();
            }
            source.Add(new UserPropVal
            {
                Code = item.Idx ?? 0,
                Name = item.Name,
                Deep = deep,
                Parent = parentCode
            });
            return source;
        }


        public async Task<int> ClassIdAsync()
        {
            await this.LoadOrgAsync();

            var c = this.Class?.FirstOrDefault();

            if (c == null)
            {
                return 0;
            }

            return c.Code;
        }



        //public 


        private bool orgLoaded = false;

        private async Task LoadOrgAsync()
        {

            if (orgLoaded)
            {
                return;
            }

            if (IsLogin is false)
            {
                return;
            }

            var rev = await _userSvc.GetOrg(this.Session);

            if (rev == null)
            {
                this.orgLoaded = true;
                return;
            }

            this.Region = new UserPropVal
            {
                Code = rev.Idx ?? 0,
                Name = rev.Name,
                Deep = 1

            };


            if (rev.ChildList == null || rev.ChildList.Count == 0)
            {
                this.orgLoaded = true;
                return;
            }

            //学校...
            foreach (var item in rev.ChildList)
            {

                this.Schools = this.AddOrgItem(this.Schools, item, rev.Idx.ToString(), 2);

                // 学段
                if (item.HasChildern() == false)
                {
                    continue;

                }

                foreach (var sectionItem in item.ChildList)
                {
                    this.Section = this.AddOrgItem(this.Section, sectionItem, item.Idx.ToString(), 3);

                    if (sectionItem.HasChildern() == false)
                    {
                        continue;
                    }

                    //年级....
                    foreach (var grade in sectionItem.ChildList)
                    {
                        this.Grade = this.AddOrgItem(this.Grade, grade, sectionItem.Idx.ToString(), 4);

                        if (grade.HasChildern() == false)
                        {
                            continue;
                        }
                        foreach (var classItem in grade.ChildList)
                        {
                            this.Class = this.AddOrgItem(this.Class, classItem, grade.Idx.ToString(), 5);
                        }
                    }
                }

            }

            this.orgLoaded = true;



        }

        public async Task<UserPropVal> Get(string orgType)
        {

            if (this.IsLogin is false)
            {
                return null;
            }
          
            await this.LoadOrgAsync();

            return orgType switch
            {
                AppUserFlagData.OrgRegion => this.Region,
                AppUserFlagData.OrgSchool => this.Schools?.FirstOrDefault(),
                AppUserFlagData.OrgGrade => this.Grade?.FirstOrDefault(),
                AppUserFlagData.OrgSection => this.Section?.FirstOrDefault(),
                _ => throw new NotImplementedException(),
            };

        }

        public Task<UserPropVal> GetRegion()
        {
            return this.Get(AppUserFlagData.OrgRegion);
            //throw new NotImplementedException();
        }

        public Task<UserPropVal> GetSchool()
        {
            return this.Get(AppUserFlagData.OrgSchool);
        }

        //public bool IsAuditor
        //{
        //    get
        //    {
        //        if (this.Roles == null)
        //        {
        //            var ret = this._userSvc.GetUserRole(this._session);

        //            if (ret == null || ret.Count == 0)
        //            {
        //                return false;
        //            }

        //            this.Roles = ret.Select(r => new UserPropVal
        //            {
        //                Code = r.Idx,
        //                Name = r.Name
        //            }).ToList();
        //        }

        //        return this.Roles.Any(r => this._config.Value.Auditor.Contains(r.Code));
        //    }
        //}

    }
}
