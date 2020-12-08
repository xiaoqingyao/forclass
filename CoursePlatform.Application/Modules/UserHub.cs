using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Modules
{


    public interface IUserHub
    {
        IAppUser AppUsr { get; }

        Task<PlatformUser> GetAsync();
    }

    public class UserHub : IUserHub
    {

        private readonly IAppUser _appuser;

        private readonly IPlatformUserService _platUser;

        public UserHub(IAppUser appuser, IPlatformUserService platUser)
        {
            _appuser = appuser;
            _platUser = platUser;
        }

        public async Task<PlatformUser> GetAsync()
        {
            if (this._appuser.UserId == 0)
            {
                throw new NotLoginException();
            }
            var sc = await this._appuser.Get(AppUserFlagData.OrgSchool);
            var re = await this._appuser.Get(AppUserFlagData.OrgRegion);
            //var grp = await this._appuser.Get(AppUserFlagData.j)
            int schoolId = sc is null ? 0 : sc.Code;
            string scName = sc is null ? "" : sc.Name;
            var rev = await this._platUser.GetOrCreate(new PltUserParam(this._appuser.UserId, this._appuser.UserName, schoolId, re.Code, scName));
            return rev;
        }

        public IAppUser AppUsr
        {
            get
            {
                return this._appuser;
            }
        }

    }
}
