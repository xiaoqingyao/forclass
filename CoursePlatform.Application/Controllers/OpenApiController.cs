using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Queries;
using CoursePlatform.Domain.Queries.OpenApi;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Controllers
{
    public class OpenApiController : CoursePlatformControllerBase
    {


        private readonly ICourseQuery _query;
        private readonly IUnitOfWork<CPDbContext> _unitOfWork;

        public OpenApiController(IAppUser user, IMapper mapper, ICourseQuery query, IUnitOfWork<CPDbContext> unitOfWork) : base(user, mapper)
        {
            _query = query;
            _unitOfWork = unitOfWork;
        }





        /// <summary>
        /// 查询课程 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("course_list")]
        [SwaggerOperation(Tags = new string[] { SwgTag_OpenAPI})]
        public async Task<ReturnVal<GetCourseListRsp>> GetList(ReqDTO dto)
        {

            var ret = await this._query.QueryAsync(new Domain.DTOS.Queries.CourseParamters()
            {

                OrgId = dto.OrgId,
                QueryType = '0',
                PageIndex = dto.PageNum -1,
                PageSize = dto.PageSize,
                //Status = (int)CourseStatus.RegionListed,
                OrgType = 'r',
            }, true);

            var rev = new GetCourseListRsp();

            if (ret.ItemCount == 0)
            {
                return this.RetOk(rev);
            }

            var items = new List<CourseInfo>();


            var courseIdAry = ret.Data.Select(s => s.Id).ToList();



            var dict = await this._unitOfWork.GetRepositoryAsync<QuoteDsEntity>()
                                .Queryable(q => courseIdAry.Contains(q.ID))
                                .GroupBy(g => g.CourseId)
                                .Select(g => new
                                {
                                    Id = g.Key,
                                    Count = g.Count()
                                })
                                .ToDictionaryAsync(q => q.Id, q => q.Count);


           
            foreach (var item in ret.Data)
            {
                CourseInfo info = new()
                {
                    Id = item.Id,
                    CreateId = item.CreatorCode.ToString(),
                    CreateName = item.CreatorName,
                    Desc = item.Intro,
                    JoinCount = item.LearnerCount,
                    Name = item.Name,
                    Status = (int)CourseStatus.RegionListed,
                    GradeIds = CourseInfo.GradeInfo(item.CatalogId, item.CatalogName),
                    Cover = item.CoverUrl, //new string[] { item.CatalogName?.Split(new char[] { '/' }).FirstOrDefault() }
                    CreationTime = item.CreationTime.HasValue ? item.CreationTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null

                };

                if (dict != null && dict.TryGetValue(item.Id, out int val))
                {
                    item.LearnerCount = dict[item.Id];
                }

                items.Add(info);
            }

            rev.Courses = items;
            rev.Totalcount = ret.ItemCount;

            return this.RetOk(rev);

        }



    }
}
