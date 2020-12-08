using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Application.Modules.Filters;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.DTOS.Queries;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Domain.Queries;
using CoursePlatform.Domain.VO;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace CoursePlatform.Application.Controllers
{
    public class QueryController : CoursePlatformControllerBase
    {

        private readonly IFilterQuery _query;
        private readonly ICourseQuery _courseQuery;
        private readonly IUnitOfWork<CPDbContext> _unitOfWrok;
        private readonly IPermission _pm;

        public QueryController(IFilterQuery query, IAppUser user, ICourseQuery courseQuery, IMapper mapper, IUnitOfWork<CPDbContext> unitOfWork, IPermission pm) : base(user, mapper)
        {
            _query = query;
            _courseQuery = courseQuery;
            _unitOfWrok = unitOfWork;
            _pm = pm;

        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("filter")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public async Task<ReturnVal<IList<TagVO>>> QueryFilter(FilterParam dto)
        {
            //var scval = await this._user.Get(AppUserFlagData.OrgSchool);
            //var regVal = await this._user.Get(AppUserFlagData.OrgRegion);
            var rev = await this._query.QueryFilterAsync(dto);
            return this.RetOk(rev);
        }


        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("tag_prop")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        [LoginFilter]
        public async Task<ReturnVal<IList<TagVO>>> GetTagProp()
        {

            var data = await this._unitOfWrok.GetRepositoryAsync<SysConfigEntity>()
                 .Queryable(_ => true)
                 .FirstOrDefaultAsync();

            if (data == null)
            {
                return null;
            }

            var tagAttr = JsonConvert.DeserializeObject<IList<TagVO>>(data.TagAttr);

            return RetOk(tagAttr);

        }

        /// <summary>
        /// 课程状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("course_status")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public async Task<ReturnVal<IDictionary<string, int>>> CourseStatusDict()
        {

            await Task.CompletedTask;

            IDictionary<string, int> rev = new Dictionary<string, int>
            {
                { "草稿", (int)(CourseStatus.Draft) },
                { "校提审", (int)(CourseStatus.Review) },
                { "校上架", (int)CourseStatus.Listed },
                {"校下架", (int)CourseStatus.UnListed },
                {"校通过", (int)CourseStatus.Accept },
                {"校拒绝", (int)CourseStatus.Reject },
                {"区域提审", (int)CourseStatus.RegionReview },
                {"区域通过", (int)CourseStatus.RegionAccept},
                {"区域上架", (int)CourseStatus.RegionListed },
                {"区域拒绝", (int)CourseStatus.RegionReject },
                {"区域下架", (int)CourseStatus.RegionUnlisted },
                {"区域状态默认", (int)CourseStatus.RegiogDefault }
            };

            return RetOk(rev);

        }




        /// <summary>
        /// 根据角色获取可以用来筛选的课程状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("enable_status")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public async Task<ReturnVal<IDictionary<string, int>>> CourseEnableStatusDict()
        {

            await Task.CompletedTask;

            if (this._pm.IsRegionAuditor)
            {
                IDictionary<string, int> rev = new Dictionary<string, int>
            {
                    {"全部", 0 },
                { "区域提审", (int)CourseStatus.RegionReview },
                { "区域通过", (int)CourseStatus.RegionAccept},

                { "区域上架", (int)CourseStatus.RegionListed },

                {"区域下架", (int)CourseStatus.RegionUnlisted }
                //,{ "区域拒绝", (int)CourseStatus.RegionReject }
        };
                return RetOk(rev);
            }

            if (this._pm.IsSchoolAuditor)
            {
                IDictionary<string, int> rev = new Dictionary<string, int>
            {
                {"全部", 0 },
                //{ "草稿", (int)(CourseStatus.Draft) },
                { "校提审", (int)(CourseStatus.Review) },
                { "校上架", (int)CourseStatus.Listed },

                {"校下架", (int)CourseStatus.UnListed },
                { "校通过", (int)CourseStatus.Accept }
                //,{ "校拒绝", (int)CourseStatus.Reject }
        };

                return RetOk(rev);
            }


            throw new CPPermissionException();

        }






        /// <summary>
        /// 课程创建页面可用的筛选状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("create_status")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public Task<ReturnVal<HashSet<int>>> CreateStatus()
        {
            var ret = this._query.EnableStatus();

            return RetOkAsync(ret);
        }



        /// <summary>
        /// 课程列表 
        /// </summary>
        /// <remarks>
        ///  按功能设置参数:queryType -> 's':学生学习， 'a':审核， 'c':教师创建 
        ///
        ///  当为’a'时，orgId 必须大于0 ，为学校Id或区域Id
        ///
        ///   orgType: s -> 学校， r -> 区域
        /// 
        /// ```
        ///  合作伙伴首页 : qeuryType=&amp;orgType={s or r}&amp;orgId={学校Id or 区域Id}&amp;orderBy=n
        /// ```
        /// ```
        ///  学习课程： queryType=s
        /// ```
        /// 
        /// ```
        ///  创建课程： qureyType=c
        /// ```
        ///
        /// ```
        ///  审核课程:  queryType=a&amp;orgId={学校 or 区域Id}
        /// ```
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("course_list")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public Task<ReturnVal<Pagnation<CoursePagingVO>>> CourseList(CourseParamters param)
        {
            var rev = this._courseQuery.QueryAsync(param);
            return this.RetOkAsync(rev);
        }



        /// <summary>
        /// 筛选后查询出的课程总数
        /// </summary>
        /// <remarks>
        ///  参数同  ```course_list``` 一致
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("course_filter_count")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public Task<ReturnVal<int>> CourseFilterCount(CourseParamters param)
        {
            var rev = this._courseQuery.TagFilterCount(param);
            return this.RetOkAsync(rev);
        }




    }
}
