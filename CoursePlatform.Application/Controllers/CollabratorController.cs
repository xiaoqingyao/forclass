using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Application.Modules.Filters;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.Domain.Queries;
using CoursePlatform.Domain.Queries.Collabrator;
using CoursePlatform.Domain.Queries.Share;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Controllers
{
    public class CollabratorController : CoursePlatformControllerBase
    {


        private readonly ICourseServices _courseSvc;
        private readonly ICollabratorQuery _query;


        public CollabratorController(IAppUser user, IMapper mapper, ICourseServices courseSvc, ICollabratorQuery query) : base(user, mapper)
        {
            this._courseSvc = courseSvc;
            this._query = query;
        }


        /// <summary>
        /// 添加课程协作者
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("collabrator_add")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })] 
        [LoginFilter]
        public Task<ReturnVal<bool>> AddCollabrator(CollabratorDTO dto)
        {
            var rev = this._courseSvc.AddCollaboratorAsync(dto.CourseId, this._user.UserId, dto);

            return this.RetOkAsync(rev);
        }



        /// <summary>
        /// 替换课程协作者
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("collabrator_replace")]
        [SwaggerOperation(Tags = new string[] {SwgTag_Manager})]
        [LoginFilter]
        public Task<ReturnVal<bool>> ReplaceCollabrator(CollabratorDTO dto)
        {
            var rev = this._courseSvc.ReplaceCollaboratorAsync(dto.CourseId, this._user.UserId, dto);

            return this.RetOkAsync(rev);
        }





        /// <summary>
        /// 查询可协作的对象
        /// </summary>
        /// <param name="dto">查询参数 type: 1-学校， 2-教研组</param>
        /// <returns></returns>
        [HttpPost]
        [Route("enable_all")]
        [LoginFilter]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public async Task<ReturnVal<QueryCollabratorVO>> EnableAll(QueryDTO dto)
        {

            var school = await this._query.EnableAsync(/*dto.Type*/CollabratorType.School, dto.CourseId);
            var community = await this._query.EnableAsync(/*dto.Type*/CollabratorType.Community, dto.CourseId);

            return RetOk(new QueryCollabratorVO 
            {
                SchoolObj = school,
                CommunityObj = community
            });
        }




        /// <summary>
        /// 查询已经分享的对象
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public Task<ReturnVal<QueryCollabratorVO>> Get(IdDto dto)
        {

            var rev = this._query.GetAsync(dto.CourseId);
            return RetOkAsync(rev);

        }




    }
}
