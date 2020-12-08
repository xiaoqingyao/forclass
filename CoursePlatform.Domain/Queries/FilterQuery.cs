using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.EFProvider.Paging;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.DTOS.Queries;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Domain.Shared;
using CoursePlatform.Domain.VO;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries
{
    public interface IFilterQuery
    {
        Task<HashSet<int>> EnableStatus();
        Task<IList<TagVO>> QueryFilterAsync(FilterParam param);
    }

    public class FilterQuery : IFilterQuery
    {




        private readonly IPermission _pm;
        private readonly IAppUser _appUser;

        private readonly IUnitOfWork<CPDbContext> _reader;

        private readonly IOptions<DomainOptions> _opt;

        public FilterQuery(IPermission pm, IUnitOfWork<CPDbContext> reader, IOptions<DomainOptions> opt, IAppUser appUser)
        {
            _pm = pm;
            _reader = reader;
            _opt = opt;
            _appUser = appUser;
        }





        public async Task<HashSet<int>> EnableStatus()
        {
            var dbStatus = await this._reader.GetRepositoryAsync<CourseEntity>()
                                .Queryable(c => c.CreatorCode == this._appUser.UserId)
                                .Select(s => new
                                {
                                    s.Status,
                                    s.RegionCode
                                })
                                .GroupBy(a => new { a.Status, a.RegionCode })
                                .ToListAsync();

            HashSet<int> hasId = new HashSet<int>();

            foreach (var item in dbStatus)
            {
                hasId.Add(item.Key.RegionCode);
                hasId.Add(item.Key.Status);
            }

            return hasId;
                                
        }






        private async Task<IList<TagVO>> InitAsync(int regionId)
        {
            var rev = new List<TagVO>();
            foreach (var item in this._opt.Value.Filter)
            {

                rev.Add(new TagVO
                {
                    Name = item
                });
            }

            var record = rev.Single(r => r.Name == ConstData.Tag_Create);


            if ((await HasOwner()))
            {
                record.AddItem(new TagVOItem
                {
                    Name = ConstData.Tag_Persional_Create,
                    Id = (int)CourseProp.Personal,
                    TypeName = ConstData.Tag_Create
                });
            }



            if ((await HasCollabrator()))
            {
                record.AddItem(new TagVOItem
                {
                    Name = ConstData.Tag_Collabrator_Create,
                    Id = (int)CourseProp.Collabrator,
                    TypeName = ConstData.Tag_Create
                });
            }

            //record.AddItem(new TagVOItem
            //{
            //    Name = ConstData.Tag_Persional_Create,
            //    Id = (int)CourseProp.Personal,
            //    TypeName = ConstData.Tag_Create
            //}).Add(new TagVOItem
            //{
            //    Name = ConstData.Tag_Collabrator_Create,
            //    Id = (int)CourseProp.Collabrator,
            //    TypeName = ConstData.Tag_Create
            //});

            ////如果有区域权限
            //if (this._pm.IsRegionAuditor)
            //{
            //    var schoolIem = await this.RegionSchool(regionId);
            //    rev.Single(f => f.Name == "学校")
            //        .AddItem(schoolIem.ToArray());
            //}


            return rev;
        }



        ///// <summary>
        ///// 区域下所有的学校
        ///// </summary>
        ///// <param name="regionId"></param>
        ///// <returns></returns>
        //private Task<List<TagVOItem>> RegionSchool(int regionId) => this._reader.GetRepositoryAsync<CourseEntity>()
        //        .Queryable(c => c.RegionCode == regionId)
        //        .Select(t => new TagVOItem
        //        {
        //            Id = t.SchoolCode,
        //            Name = t.SchoolName

        //        })
        //        .Distinct()
        //        .ToListAsync();




        public async Task<IList<TagVO>> QueryFilterAsync(FilterParam param)
        {

            Expression<Func<TagsEntity, bool>> filter = t => true;

            //if (_pm.IsRegionAuditor)
            //{
            //    filter = filter.And(t => t.RegtionId == regionId);
            //}
            //else if (_pm.IsSchoolAuditor)
            //{
            //    filter = filter.And(t => t.SchoolId == schoolId);
            //}
            //else
            //{
            //    filter = filter.And(t => t.Creator == userId);
            //}


            IQueryable<string> courseId = null;


            //如果是管理员审核
            if (param.QueryType == ConstData.Filter_CourseAudit)
            {
                if (this._pm.IsSchoolAuditor)
                {

                    var sc = await this._appUser.GetSchool();

                    filter = filter.And(t => t.SchoolId == sc.Code);
                }
                if (this._pm.IsRegionAuditor)
                {
                    var re = await this._appUser.GetRegion();
                    filter = filter.And(t => t.RegtionId == re.Code);
                }

                Expression<Func<CourseEntity, bool>> courseFilter = _ => true;

                courseFilter = await QueryTools.FilterByPermissionAsync(courseFilter, _appUser, _pm);

                courseId = this._reader.GetRepositoryAsync<CourseEntity>()
                    .Queryable(courseFilter)
                    .Select(c => c.ID);


            }


            //个人
            if (param.QueryType is ConstData.Filter_CourseQuery)
            {
                if (this._appUser.IsLogin is false)
                {
                    throw new CPPermissionException("无权限、请登录");
                }

                var collabratorQuery = this._reader.GetRepositoryAsync<CollabratorEntity>()
                                                       .Queryable(c => c.ObjId == this._appUser.UserId)
                                                       .Select(c => c.CourseId);

                filter = filter.And(t => t.Creator == this._appUser.UserId || collabratorQuery.Contains(t.CourseId));


            }


            var query = this._reader.GetRepositoryAsync<TagsEntity>().Queryable(filter);


            //如果是全部，未登录只显示上架的产品，反之显示上架和通过的
            if (param.QueryType is ConstData.Filter_All)
            {

                Expression<Func<CourseEntity, bool>> subFilter = _ => true;

                var (re, sc) = await this._appUser.GetReScCodeAsync();

                subFilter = QueryTools.QueryByAuthor(subFilter, this._appUser.IsLogin, param.OrgType, param.OrgId, re,sc);

                var subQuery = this._reader.GetRepositoryAsync<CourseEntity>()
                    .Queryable(subFilter)
                    .Select(c => c.ID);



                query = query.Join(subQuery, t => t.CourseId, c => c, (t, c) => t);
            }



            //学习
            if (param.QueryType is ConstData.Filter_Learning)
            {
                if (this._appUser.IsLogin is false)
                {
                    throw new CPPermissionException("无权限、请登录");
                }

                // 加入学习的课程...
                var subQuery = this._reader.GetRepositoryAsync<PlatformUserJoinedCourseEntity>()
                    .Queryable(t => t.UserId == this._appUser.UserId)
                    .Select(p => p.CourseId)
                    .Distinct();

                query = query.Join(subQuery, t => t.CourseId, p => p, (t, p) => t);
            }


            //审核过滤
            if (courseId is not null)
            {
                query = query.Join(courseId, t => t.CourseId, c => c, (t, c) => t);
            }

            //if (param.QueryType is ConstData.Filter_CourseQuery)
            //{
            //   var collabratorQuery = this._reader.GetRepositoryAsync<CollabratorEntity>()
            //                            .Queryable(c => c.ObjId == this._appUser.UserId)
            //                            .Select(c => c.CourseId);

            //    query = query.Join(collabratorQuery, t => t.CourseId, c => c, (t, c) => t);
            //}


            var items = await query
                .GroupBy(t => new { t.Name, /*t.CategoryId, */t.TypeName })
                .Select(t => new TagsEntity
                {

                    Name = t.Key.Name,
                    TypeName = t.Key.TypeName,
                    //CategoryId = t.Key.CategoryId
                }).ToListAsync();


            //第一层
            var rev = await this.InitAsync(param.OrgId);

            if (items.NoData())
            {
                return (await CheckReturnAsync(rev)); //rev?.Where(r => r.Items is not null and { Count: > 0 }).ToList();
            }


            //根据第一层属性，添加相应标签
            foreach (var item in items)
            {
                if (item.TypeName is null or { Length: <= 0 })
                {
                    rev.FirstOrDefault(r => r.Name == ConstData.Tag_Course)?.AddItem(new TagVOItem
                    {
                        Name = item.Name,
                        Id = item.AssetId,
                        TypeName = ConstData.Tag_Course

                    });

                    continue;
                }

                rev.FirstOrDefault(r => r.Name == item.TypeName)?.AddItem(new TagVOItem
                {
                    Name = item.Name,
                    Id = item.AssetId,
                    TypeName = item.TypeName
                });

            }

            return (await CheckReturnAsync(rev)); //rev?.Where(r => r.Items is not null and { Count: > 0 }).ToList();
        }



        private Task<bool> HasCollabrator()
        {
            return this._reader.GetRepositoryAsync<CollabratorEntity>()
                .AnyAsync(c => c.ObjId == this._appUser.UserId);
        }

        private Task<bool> HasOwner()
        {
            return this._reader.GetRepositoryAsync<CourseEntity>()
                .AnyAsync(c => c.CreatorCode == this._appUser.UserId);
        }


        private async Task<IList<TagVO>> CheckReturnAsync(IList<TagVO> tags)
        {
            if (tags is null or { Count: <= 0 })
            {
                return null;
            }
            var ret = tags.Where(r => r.Items is not null and { Count: > 0 }).ToList();

         
            return ret;

        }


    }
}
