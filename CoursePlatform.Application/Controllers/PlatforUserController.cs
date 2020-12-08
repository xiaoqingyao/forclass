using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Application.Modules.Filters;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Tools;
using LinqToDB.Data.RetryPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Controllers
{
    public class PlatforUserController : CoursePlatformControllerBase
    {



       private readonly IPlatformUserService _platUsr;

        private readonly IUserHub _hub;

        private readonly ICourseServices _courseSvc;



        public PlatforUserController(IPlatformUserService platUsr, IUserHub hub, ICourseServices courseSvc, IAppUser user, IMapper mapper) :base(user, mapper)
        {
            _platUsr = platUsr;
            _hub = hub;
            _courseSvc = courseSvc;
        }


        /// <summary>
        /// 加入学习
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("join")]
        [SwaggerOperation(Tags = new string[] { SwgTag_User })]
        [LoginFilter]
        public async Task<ReturnVal<bool>> Join(UserJoinDTO dto)
        {

            var pltUsr = await this._hub.GetAsync();

            var rev = await this._courseSvc.LeanerJoin(pltUsr.UserId, pltUsr.ID, dto.CourseId);

            if (rev == null)
            {
                return RetErr<bool>("error");
            }

            var ret = await this._platUsr.JoinCourse(pltUsr.UserId, dto.CourseId, rev.Creator.Code);

            return RetOk(ret);

                       
        }




        /// <summary>
        /// 取消加入学习的学程
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("leave")]
        [SwaggerOperation(Tags = new string[] { SwgTag_User })]
        [LoginFilter]
        public async Task<ReturnVal<bool>> Leavel(IdDto dto)
        {
            var rev = await this._courseSvc.LeanerLeave(this._user.UserId, dto.CourseId);
            if (rev== null)
            {
                return RetOk(false);
            }
            bool ret = await this._platUsr.LeaveCourse(this._user.UserId, dto.CourseId, rev.Creator.Code);
            return RetOk(ret);
        }



    }
}
