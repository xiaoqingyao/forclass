using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoursePlatform.Application.Modules;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.Core.PlatformUserAggregate;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.Domain.DTOS.Queries;
using CoursePlatform.Domain.VO;
using CoursePlatform.Events.Events;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Tools;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NLog.Web.LayoutRenderers;
using Swashbuckle.AspNetCore.Annotations;
using CoursePlatform.infrastructure.Validators;
using CoursePlatform.Application.Modules.Filters;
using CoursePlatform.Domain.Permission;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.Domain.Commands;
using CoursePlatform.Domain.Core.PartnerAggregate;

namespace CoursePlatform.Application.Controllers
{



    public class CourseController : CoursePlatformControllerBase
    {



        private readonly ICourseServices _courseSvc;
        private readonly IPlatformUserService _puserSvc;
        private readonly IPartnerService _partnerSvc;
        private readonly IPermission _pm;
        private readonly IIDTools _idTool;

        public CourseController(ICourseServices courseSvc, IPlatformUserService puserSvc, IMapper mapper, IAppUser user, IPartnerService partnerSvc, IPermission pm, IIDTools idTool) : base(user, mapper)
        {
            _courseSvc = courseSvc;
            _puserSvc = puserSvc;
            _partnerSvc = partnerSvc;
            _pm = pm;
            _idTool = idTool;
        }









        /// <summary>
        ///  课程创建 
        /// </summary>
        /// <param name="dTO">课程数据  <see cref="CreateCourseDTO"/></param>
        /// <returns>
        /// 创建成功的ID， <see cref="ReturnVal{T}"/> 
        /// </returns>
        /// <remarks>
        ///     ID为空时表示创建，反之表示修改
        /// </remarks>
        [HttpPost]
        [SwaggerOperation(Tags = new string[] { "课程管理" })]
        [LoginFilter]
        public async Task<ReturnVal<string>> Create(CreateCourseDTO dTO)
        {



            var region = await this._user.Get(AppUserFlagData.OrgRegion);
            var school = await this._user.Get(AppUserFlagData.OrgSchool);
            var creator = new UserPropVal
            {
                Code = this._user.UserId,
                Name = this._user.UserName
            };

            string rev;

            if (String.IsNullOrEmpty(dTO.Id))
            {


                var id = _idTool.ID();

                var sc = await this._user.GetSchool();

                var userRev = await this._puserSvc.CreateCourse(new PltUserParam(this._user.UserId, this._user.UserName, sc.Code, (await this._user.GetRegion()).Code,sc.Name), id);
                if (userRev is false)
                {
                    return this.RetErr<string>("用户信息异常");
                }

                rev = await this._courseSvc.CreateAsync(new CreateCourseCommand(dTO) { CommandId = id }, region, school, creator);
            }
            else
            {
                rev = await this._courseSvc.UpdateAsunc(dTO, this._user.UserId);
            }


            return this.RetOk(rev);

        }


        /// <summary>
        /// 获取课程信息...
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public async Task<ReturnVal<CourseVO>> Get(GetParam param)
        {
            var course = await this._courseSvc.Get(param.Id);

            if (course == null)
            {
                return this.RetErr<CourseVO>("信息不存在");
            }

            var rev = this._mapper.Map<CourseVO>(course);
            rev.CreationDate = course.CreationTime?.ToString("yyyy.MM.dd");
            rev.IsJoined = course.IsJoined(this._user.UserId);

            var collabrator = course.CollaboratorLib?.Value?.Items?.Keys.ToList();

            await rev.SetOperationValAsync(new ValidateParameter(course.School.Code,
                                                                 course.Region.Code,
                                                                 course.Creator.Code,
                                                                 collabrator,
                                                                 course.Status,
                                                                 course.RegionStatus), this._pm, this._mapper);         


            return this.RetOk(rev);

        }

        /// <summary>
        /// 获取课程某目录下引用的学程
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get_ds")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Qeury })]
        public async Task<ReturnVal<IList<DsItemVO>>> GetDs(GetDsParam param)
        {
            var course = await this._courseSvc.Get(param.Id);

            if (course == null)
            {
                return this.RetErr<IList<DsItemVO>>("信息不存在");
            }



            if (course.DsItems.NoData())
            {

                return this.RetErr<IList<DsItemVO>>("引用的学程不存在");
            }

            var dsCatalog = course.DsItems.Where(d => d.CatalogId == param.CatalogId).OrderBy(d => d.SortVal);

            if (dsCatalog.NoData())
            {

                return this.RetErr<IList<DsItemVO>>("该目录引用的学程不存在");
            }


            var rev = this._mapper.Map<IList<DsItemVO>>(dsCatalog);

            if (this._user.IsLogin && course.IsJoined(this._user.UserId))
            {
                ((List<DsItemVO>)rev).ForEach(item => item.IsOpen = true);
            }

            return this.RetOk(rev);

        }

        /// <summary>
        /// x引用学程
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("qoute_ds")]
        [SwaggerOperation(Tags = new string[] { "课程管理" })]
        [LoginFilter]
        public Task<ReturnVal<bool>> QuoteDS(QuoteDSDTO dto)
        {
            var rev = this._courseSvc.QoutesDSAsync(dto, this._user.UserId, this._user.UserName);
            return this.RetOkAsync(rev);
        }




        /// <summary>
        /// 设置学程状态
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ds_status")]
        [SwaggerOperation(Tags = new string[] { "课程管理" })]
        [LoginFilter]
        public Task<ReturnVal<bool>> SetDsStatus(DsStatusDTO dto)
        {
            var rev = this._courseSvc.SetDsStatus(dto.CourseId, dto.CatalogId, dto.DsId, dto.IsOpen, this._user.UserId);

            return this.RetOkAsync(rev);
        }


        /// <summary>
        /// 删除学程的引用 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ds_del")]
        [SwaggerOperation(Tags = new string[] { "课程管理" })]
        [LoginFilter]
        public Task<ReturnVal<bool>> DelDs(DsDelDTO dto)
        {
            var rev = this._courseSvc.DelDs(dto.CourseId, dto.CatalogId, dto.DsId, this._user.UserId);

            return this.RetOkAsync(rev);
        }



        ///// <summary>
        ///// 添加课程协作者
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("collabrator_add")]
        //[SwaggerOperation(Tags = new string[] { "课程管理" })]
        //[LoginFilter]
        //public Task<ReturnVal<bool>> AddCollabrator(CollabratorDTO dto)
        //{
        //    var rev = this._courseSvc.AddCollaboratorAsync(dto.CourseId, this._user.UserId, dto);

        //    return this.RetOkAsync(rev);
        //}



        ///// <summary>
        ///// 替换课程协作者
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("collabrator_replace")]
        //[SwaggerOperation(Tags = new string[] { "课程管理" })]
        //[LoginFilter]
        //public Task<ReturnVal<bool>> ReplaceCollabrator(CollabratorDTO dto)
        //{
        //    var rev = this._courseSvc.ReplaceCollaboratorAsync(dto.CourseId, this._user.UserId, dto);

        //    return this.RetOkAsync(rev);
        //}






        #region 校审核..........
        /// <summary>
        /// 校提审 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("school_review")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public Task<ReturnVal<bool>> SchoolReview(IdDto dto)
        {
            var rev = this._courseSvc.SchoolReview(dto.CourseId, this._user.UserId);
            return this.RetOkAsync(rev);
        }


        /// <summary>
        /// 取消校提审 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("school_review_cancel")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public Task<ReturnVal<bool>> SchoolReviewCancel(IdDto dto)
        {
            var rev = this._courseSvc.SchoolReviewCancel(dto.CourseId, this._user.UserId);
            return this.RetOkAsync(rev);
        }





        /// <summary>
        /// 校审核通过 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("school_pass")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public async Task<ReturnVal<bool>> SchoolPass(AuditDTO dto)
        {

            var sc = await this._user.Get(AppUserFlagData.OrgSchool);

            await this._courseSvc.SchoolPassedAsync(new CourseAuditCommand(dto.CourseID
                , this._user.UserId
                , this._user.UserName
                , sc.Code
                , sc.Name
                , dto.Desc
                ));

            return RetOk(true);

        }



        /// <summary>
        /// 校审核拒绝
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("school_reject")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public async Task<ReturnVal<bool>> SchoolReject(AuditDTO dto)
        {

            var sc = await this._user.Get(AppUserFlagData.OrgSchool);

            await this._courseSvc.SchoolRejectAsync(new CourseAuditCommand(dto.CourseID
                , this._user.UserId
                , this._user.UserName
                , sc.Code
                , sc.Name
                , dto.Desc
                ));

            return RetOk(true);

        }




        /// <summary>
        /// 学校上架
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list_school")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        public async Task<ReturnVal<bool>> ListToSchool(AuditDTO dto)
        {
            var sc = await this._user.Get(AppUserFlagData.OrgSchool);
            var rc = await this._user.Get(AppUserFlagData.OrgRegion);
            var ret = await this._courseSvc.List2SchoolAsync(new CourseAuditCommand(dto.CourseID, this._user.UserId, this._user.UserName, sc.Code, sc.Name, dto.Desc));
            await this._partnerSvc.CourseToList(ret.Creator.Code,dto.CourseID, sc.Name, sc.Code, rc.Code);
            await this._puserSvc.ListedCourse(ret.Creator.Code, dto.CourseID);
            return this.RetOk(true);
        }


        /// <summary>
        /// 学校上架
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list_school_remove")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        public async Task<ReturnVal<bool>> ListToSchoolRemove(AuditDTO dto)
        {
            var sc = await this._user.Get(AppUserFlagData.OrgSchool);
            //_ = await this._user.Get(AppUserFlagData.OrgRegion);
            var course = await this._courseSvc.UnList2SchoolAsync(new CourseAuditCommand(dto.CourseID, this._user.UserId, this._user.UserName, sc.Code, sc.Name, dto.Desc));
            await this._partnerSvc.CourselistRemove(dto.CourseID, sc.Code,PartnerType.School);
            await this._puserSvc.ListCourseRemove(course.Creator.Code, dto.CourseID);
            return this.RetOk(true);
        }


        #endregion



        #region 区域审核


        /// <summary>
        /// 区域提审 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("region_review")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public Task<ReturnVal<bool>> RegionReview(IdDto dto)
        {
            var rev = this._courseSvc.RegionReview(dto.CourseId, this._user.UserId);
            return this.RetOkAsync(rev);
        }


        /// <summary>
        /// 取消区域提审 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("region_review_cancel")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public Task<ReturnVal<bool>> RegionReviewCancel(IdDto dto)
        {
            var rev = this._courseSvc.RegionReviewCancel(dto.CourseId, this._user.UserId);
            return this.RetOkAsync(rev);
        }





        /// <summary>
        /// 区域审核通过 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("region_pass")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public async Task<ReturnVal<bool>> RegionPass(AuditDTO dto)
        {

            var sc = await this._user.Get(AppUserFlagData.OrgRegion);

            await this._courseSvc.RegionPassedAsync(new CourseAuditCommand(dto.CourseID
                , this._user.UserId
                , this._user.UserName
                , sc.Code
                , sc.Name
                , dto.Desc
                ));

            return RetOk(true);

        }



        /// <summary>
        /// 区域审核拒绝
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("region_reject")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        [LoginFilter]
        public async Task<ReturnVal<bool>> RegionReject(AuditDTO dto)
        {

            var sc = await this._user.Get(AppUserFlagData.OrgRegion);

            await this._courseSvc.RegionRejectAsync(new CourseAuditCommand(dto.CourseID
                , this._user.UserId
                , this._user.UserName
                , sc.Code
                , sc.Name
                , dto.Desc
                ));

            return RetOk(true);

        }




        /// <summary>
        /// 区域上架
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list_region")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        public async Task<ReturnVal<bool>> ListToRegion(AuditDTO dto)
        {
            var sc = await this._user.Get(AppUserFlagData.OrgRegion);
            var rev = await this._courseSvc.List2RegionAsync(new CourseAuditCommand(dto.CourseID, this._user.UserId, this._user.UserName, sc.Code, sc.Name, dto.Desc));
            await this._partnerSvc.CourseToList(rev.Creator.Code,dto.CourseID, sc.Name, sc.Code, 0) ;

            await this._puserSvc.ListedCourse(rev.Creator.Code, dto.CourseID);
            return this.RetOk(true);



        }

        /// <summary>
        /// 学校上架
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list_region_remove")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        public async Task<ReturnVal<bool>> ListToRegionRemove(AuditDTO dto)
        {
            var sc = await this._user.GetRegion();
            //_ = await this._user.Get(AppUserFlagData.OrgRegion);
            var coures = await this._courseSvc.UnList2RegionAsync(new CourseAuditCommand(dto.CourseID, this._user.UserId, this._user.UserName, sc.Code, sc.Name, dto.Desc));
            await this._partnerSvc.CourselistRemove(dto.CourseID, sc.Code, PartnerType.Region);
            await this._puserSvc.ListCourseRemove(coures.Creator.Code, dto.CourseID);
            return this.RetOk(true);
        }

        #endregion





        /// <summary>
        /// 删除课程 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <remarks>
        ///  只有在草稿、校拒绝、校提审状态才可以删除 
        /// </remarks>
        [HttpPost]
        [Route("del")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        public Task<ReturnVal<bool>> Delete(IdDto dto)
        {
            var rev = this._courseSvc.Delete(dto.CourseId, this._user.UserId);

            return RetOkAsync(rev);
        }





        /// <summary>
        /// 更新指定引用学程的基本信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update_ds_item")]
        [SwaggerOperation(Tags = new string[] { SwgTag_Manager })]
        public Task<ReturnVal<bool>> UpdateDsItem(UpdateDsDTO dto)
        {

            var rev = this._courseSvc.UpdateDs(dto, this._user.UserId);

            return RetOkAsync(rev);
        }





    }
}
