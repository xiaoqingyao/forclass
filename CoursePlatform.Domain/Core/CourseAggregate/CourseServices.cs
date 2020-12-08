using Autofac.Features.Indexed;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Commands;
using CoursePlatform.Domain.Core.ValueFactory;
using CoursePlatform.Domain.DTOS;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Events;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Validators;
using CoursePlatform.Infrastructure.Caching;
using Microsoft.Extensions.Options;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.CourseAggregate
{



    public interface ICourseServices
    {
        Task<bool> AddCollaboratorAsync(string courseId, int operatorId, CollabratorDTO dto);
        Task<string> CreateAsync(CreateCourseCommand cmd, UserPropVal region, UserPropVal school, UserPropVal creator);
        Task<bool> DelDs(string courseId, int catalogId, Guid dsId, int operatorId);
        Task<bool> Delete(string courseId, int operaoterId);
        Task<Course> Get(string id);
        Task<Course> LeanerJoin(int userId, string pltUserId, string courseId);
        Task<Course> LeanerLeave(int userId, string courseId);
        Task<Course> List2RegionAsync(CourseAuditCommand cmd, Course root = null);
        Task<Course> List2SchoolAsync(CourseAuditCommand cmd, Course root = null);
        Task<bool> QoutesDSAsync(QuoteDSDTO dto, int operatorId, string operatorName);
        Task RegionPassedAsync(CourseAuditCommand cmd, Course root = null);
        Task RegionRejectAsync(CourseAuditCommand cmd, Course root = null);
        Task<bool> RegionReview(string courseId, int operatorId);
        Task<bool> RegionReviewCancel(string courseId, int operatorId);
        Task<bool> ReplaceCollaboratorAsync(string courseId, int operatorId, CollabratorDTO dto);
        Task SchoolPassedAsync(CourseAuditCommand cmd, Course root = null);
        Task SchoolRejectAsync(CourseAuditCommand cmd, Course root = null);
        Task<bool> SchoolReview(string courseId, int operatorId);
        Task<bool> SchoolReviewCancel(string courseId, int operatorId);
        Task<bool> SetDsStatus(string courseId, int catalogId, Guid dsId, bool isOpen, int operatorId);
        Task<Course> UnList2RegionAsync(CourseAuditCommand cmd, Course root = null);
        Task<Course> UnList2SchoolAsync(CourseAuditCommand cmd, Course root = null);
        Task<string> UpdateAsunc(CreateCourseDTO dto, int editorId);
        Task<bool> UpdateDs(UpdateDsDTO dto, int userId);
    }

    public class CourseServices : ICourseServices
    {


        private readonly IToolbox toolbox;
        private readonly IPermission _pm;
        private readonly IValueFactory _vf;
        private readonly ICourseLoader _loader;

        public CourseServices(IToolbox toolbox, IPermission pm, IValueFactory vf, ICourseLoader loader)
        {
            this.toolbox = toolbox;
            this._pm = pm;
            this._vf = vf;
            this._loader = loader;
        }


        //#region  internal function....

        //private string cacheKey(string id)
        //{
        //    return String.Concat(this.toolbox.Options.Value.CourseCachePrefix, id);
        //}



        //private async Task<Course> loaderAsync(string id)
        //{


        //    var obj = await this.toolbox.Cachor.Get<Course>(cacheKey(id), async () =>
        //    {

        //        var entity = await this.toolbox.Reader.GetRepositoryAsync<CourseEntity>()
        //                .SingleAsync(C => C.ID == id);

        //        var rev = this.toolbox.Mapper.Map<Course>(entity);

        //        if (rev == null)
        //        {
        //            return null;
        //        }

        //        rev.Region = new UserPropVal
        //        {
        //            Code = entity.RegionCode,
        //            Name = entity.RegionName

        //        };
        //        rev.School = new UserPropVal
        //        {
        //            Name = entity.SchoolName,
        //            Code = entity.SchoolCode

        //        };
        //        rev.Creator = new UserPropVal
        //        {
        //            Code = entity.CreatorCode,
        //            Name = entity.CreatorName
        //        };

        //        //标签
        //        var tag = await this.toolbox.Reader.GetRepositoryAsync<TagsEntity>()
        //                    .Query(t => t.CourseId == id);

        //        rev.Tag = this.TagFromEntity(tag);





        //        //引用的学程
        //        var ds = await this.toolbox.Reader.GetRepositoryAsync<QuoteDsEntity>()
        //                .Query(q => q.CourseId == id);

        //        rev.DsItems = this.DsFromEntity(ds);


        //        ////引用人
        //        //var collabrators = await this.toolbox.Reader.GetRepositoryAsync<CollabratorEntity>()
        //        //                .Query(c => c.CourseId == id);

        //        //if (collabrators.NoData() is false)
        //        //{
        //        //    rev.Collaborator = new Lazy<CollabratorVal>(() =>
        //        //    { //rev.Collaborator.Binder();

        //        //        var rev = new CollabratorVal();
        //        //        foreach (var item in collabrators)
        //        //        {
        //        //            rev.Add(new(item.ObjId));
        //        //        }
        //        //        return rev;
        //        //    });
        //        //}

        //        return rev;

        //    });


        //    if (obj is null)
        //    {
        //        return null;
        //    }



        //    this._vf.Register(obj, obj => obj.CollaboratorLib, async () => {

        //        //引用人
        //        var collabrators = await this.toolbox.Reader.GetRepositoryAsync<CollabratorEntity>()
        //                        .Query(c => c.CourseId == id);

        //        if (collabrators.NoData() is false)
        //        {
                  
        //                var rev = new CollabratorVal();
        //                foreach (var item in collabrators)
        //                {
        //                  rev.Add(new(item.ObjId));
        //                }
        //                return rev;
        //        }

        //        return null;
        //    });



        //    return obj;
        //}


        //private IList<QuoteDSVal> DsFromEntity(IList<QuoteDsEntity> entities)
        //{
        //    if (entities == null || entities.Count == 0)
        //    {
        //        return null;
        //    }

        //    var rev = new List<QuoteDSVal>();

        //    foreach (var item in entities)
        //    {
        //        rev.Add(new QuoteDSVal
        //        {
        //            IsOpen = item.IsOpen,
        //            CatalogId = item.CatalogId,
        //            DsId = item.DsId,
        //            DsName = item.DsName,
        //            OperatorId = item.OperatorId,
        //            OperatorName = item.OperatorName,
        //            SortVal = item.SortVal,
        //            IsOriginal = item.IsOriginal,
        //            IsShared = item.IsShared
        //        });
        //    }
        //    return rev;
        //}

        //private TagVal TagFromEntity(IList<TagsEntity> entity)
        //{

        //    if (entity == null || entity.Count == 0)
        //    {
        //        return null;
        //    }

        //    var tagVal = new TagVal
        //    {
        //        Items = new List<TagItem>()
        //    };


        //    foreach (var item in entity)
        //    {

        //        if (String.IsNullOrEmpty(item.TypeName))
        //        {
        //            tagVal.Name = item.Name;
        //            tagVal.Id = item.AssetId;
        //            continue;
        //        }
        //        tagVal.Items.Add(new TagItem
        //        {
        //            Name = item.Name,
        //            TypeName = item.TypeName
        //        });

        //    }

        //    return tagVal;
        //}

        //private async Task<bool> save(Course course)
        //{
        //    bool rev = await this.toolbox.Cachor.SetAsync(this.cacheKey(course.ID), course);
        //    if (rev)
        //    {
        //        await this.toolbox.Sender.SendAsync(course.Events);
        //    }

        //    return rev;

        //}

        //#endregion

        public async Task<string> CreateAsync(CreateCourseCommand cmd, UserPropVal region, UserPropVal school, UserPropVal creator)
        {

            var course = this.toolbox.Mapper.Map<Course>(cmd.dTO);

            course.ID = cmd.CommandId;

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.Create(region, school, creator);

            await this._loader.Save(course);


            return course.ID;

        }


        public async Task<string> UpdateAsunc(CreateCourseDTO dto, int editorId)
        {

            var course = await this._loader.LoaderAsync(dto.Id);


            if (course == null)
            {
                throw new CPValidateExceptions($"课程不存在..{dto.Id}");
            }


            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);



            course.Update(dto, editorId);


            await this._loader.Save(course);

            return course.ID;

        }

        public Task<Course> Get(string id)
        {
            return this._loader.LoaderAsync(id);
        }


        public async Task<bool> QoutesDSAsync(QuoteDSDTO dto, int operatorId, string operatorName)
        {

            var coures = await this._loader.LoaderAsync(dto.CourseId);
            if (coures == null)
            {
                throw new CPValidateExceptions($"课程信息不存在:{dto.CourseId}");
            }

            coures.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            var rev = coures.QuoteDS(dto, operatorId, operatorName);

            if (rev is false)
            {
                return false;
            }

            await this._loader.Save(coures);

            return true;
        }


        public async Task<bool> SetDsStatus(string courseId, int catalogId, Guid dsId, bool isOpen, int operatorId)
        {
            var course = await this._loader.LoaderAsync(courseId);
            if (course == null)
            {
                throw new CPValidateExceptions($"课程信息不存在{courseId}");
            }
            if (course.Creator.Code != operatorId)
            {
                throw new CPPermissionException($"无权限进行此操作");
            }
            bool rev = course.SetDsStatus(catalogId, dsId, isOpen);
            if (!rev)
            {
                return false;
            }
            await this._loader.Save(course);
            return true;

        }


        public async Task<bool> DelDs(string courseId, int catalogId, Guid dsId, int operatorId)
        {
            var course = await this._loader.LoaderAsync(courseId);

            Prosecutor.NotNull(course, $"课程信息为空:{courseId}");


            bool result = course.DelDs(catalogId, dsId, operatorId);

            if (!result)
            {
                return false;
            }

            await this._loader.Save(course);

            return true;
        }

        public async Task<bool> AddCollaboratorAsync(string courseId, int operatorId, CollabratorDTO dto)
        {
            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空:{courseId}");
            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);
            course.AddCollaborator(dto, operatorId);
            await this._loader.Save(course);

            this._vf.Save(course, c=> c.CollaboratorLib);


            return true;

        }

        public async Task<bool> ReplaceCollaboratorAsync(string courseId, int operatorId, CollabratorDTO dto)
        {
            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空:{courseId}");
            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);
            course.ReplaceCollaborator(dto, operatorId);
            await this._loader.Save(course);

            this._vf.Save(course, c => c.CollaboratorLib);

            return true;

        }


        public async Task<Course> LeanerJoin(int userId, string pltUserId, string courseId)
        {
            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空");
            //if (course.Status != CourseStatus.Listed || course.RegionStatus != CourseStatus.RegionListed)
            //{
            //    throw new CPValidateExceptions("无法加入未上架的课程");
            //}
            var rev = course.JoinLeaner(userId, pltUserId);
            if (rev == false)
            {
                return null;
            }
            await this._loader.Save(course);
            return course;
        }

        public async Task<Course> LeanerLeave(int userId, string courseId)
        {
            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空");
            var rev = course.LeanerLeave(userId);
            if (rev == false)
            {
                return null;
            }
            await this._loader.Save(course);
            return course;
        }

        #region 校审核.............
        public async Task<bool> SchoolReview(string courseId, int operatorId)
        {
            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空");
            bool rev = course.SchoolReview(operatorId);
            if (rev == false)
            {
                return false;
            }
            await this._loader.Save(course);
            return true;
        }


        public async Task<bool> SchoolReviewCancel(string courseId, int operatorId)
        {


            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空");
            bool rev = course.SchoolReviewCancel(operatorId);
            if (rev == false)
            {
                return false;
            }
            await this._loader.Save(course);
            return true;
        }

        public async Task SchoolPassedAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsSchoolAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.School.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.SchoolPass(cmd);

            await this._loader.Save(course);
        }


        public async Task SchoolRejectAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsSchoolAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.School.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.SchoolReject(cmd);

            await this._loader.Save(course);
        }


        public async Task<Course> List2SchoolAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsSchoolAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.School.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.ListToSchool(cmd);

            await this._loader.Save(course);

            return course;
        }

        public async Task<Course> UnList2SchoolAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsSchoolAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.School.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.UnListToSchool(cmd);

            await this._loader.Save(course);

            return course;
        }




        #endregion



        #region 区域审核...


        public async Task<bool> RegionReview(string courseId, int operatorId)
        {
            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空");

            if ((await this._pm.IsSchoolAuditorAsync(course.School.Code)) is false)
            {
                throw new CPPermissionException();
            }

            bool rev = course.RegionReview(operatorId);
            if (rev == false)
            {
                return false;
            }
            await this._loader.Save(course);
            return true;
        }


        public async Task<bool> RegionReviewCancel(string courseId, int operatorId)
        {


            var course = await this._loader.LoaderAsync(courseId);
            Prosecutor.NotNull(course, $"课程信息为空");

            if ((await this._pm.IsSchoolAuditorAsync(course.School.Code)) is false)
            {
                throw new CPPermissionException();
            }


            bool rev = course.RegionReviewCancel(operatorId);
            if (rev == false)
            {
                return false;
            }
            await this._loader.Save(course);
            return true;
        }

        public async Task RegionPassedAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsRegionAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.Region.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.RegionPass(cmd);

            await this._loader.Save(course);
        }


        public async Task RegionRejectAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsRegionAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.Region.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.RegionReject(cmd);

            await this._loader.Save(course);
        }


        public async Task<Course> List2RegionAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsRegionAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.Region.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.ListToRegion(cmd);

            await this._loader.Save(course);

            return course;

        }

        public async Task<Course> UnList2RegionAsync(CourseAuditCommand cmd, Course root = null)
        {

            if (this._pm.IsRegionAuditor == false)
            {
                throw new CPPermissionException();
            }

            var course = root ?? await this._loader.LoaderAsync(cmd.CourseId);

            Prosecutor.NotNull(course, $"课程信息为空");

            if (course.Region.Code != cmd.ReviewerOrgId)
            {
                throw new CPPermissionException();
            }

            course.Apply(this.toolbox.DomainSetter[DomainSetter.Mapper]);

            course.UnListToRegion(cmd);

            await this._loader.Save(course);

            return course;
        }


        #endregion



        #region 删除....
        public  Task<bool> Delete(string courseId, int operaoterId)
        {
            return this._loader.Delete(courseId, operaoterId);    
        }
        #endregion


        #region 修改某个引用学程信息


        public async Task<bool> UpdateDs(UpdateDsDTO dto, int userId)
        {
            var course = await this._loader.LoaderAsync(dto.CourseId);
            Prosecutor.NotNull(course, "课程信息不存在");
            bool result = course.UpdateDs(dto, userId);
            if (result is false)
            {
                return false;
            }

            await this._loader.Save(course);

            return true;
            
        }

        #endregion




    }

}
