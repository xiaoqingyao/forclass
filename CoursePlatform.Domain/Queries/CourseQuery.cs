using AutoMapper;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.EFProvider.Paging;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.DTOS.Queries;
using CoursePlatform.Domain.Permission;
using CoursePlatform.Domain.VO;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries
{

    public interface ICourseQuery
    {
        Task<Pagnation<CoursePagingVO>> QueryAsync(CourseParamters param, bool isOpenApi = false);
        //Task<int> TagFilterCount(IList<TagParam> tags, int creatorId);
        Task<int> TagFilterCount(CourseParamters param);
    }



    public class CourseQuery : ICourseQuery
    {

        private readonly IUnitOfWork<CPDbContext> _reader;

        private readonly IAppUser _appUsr;

        private readonly IPermission _pm;

        private readonly IMapper _mp;





        public CourseQuery(IUnitOfWork<CPDbContext> reader, IAppUser appUsr, IPermission pm, IMapper mp)
        {
            _reader = reader;
            _appUsr = appUsr;
            _pm = pm;
            _mp = mp;
        }

        private Expression<Func<CourseEntity, CourseEntity>> selector = c => new CourseEntity
        {
            ID = c.ID,
            Name = c.Name,
            CoverUrl = c.CoverUrl,
            SchoolName = c.SchoolName,
            SignatureName = c.SignatureName,
            Status = c.Status,
            SchoolCode = c.SchoolCode,
            RegionCode = c.RegionCode,
            CreatorCode = c.CreatorCode,
            CollbratorCount = c.CollbratorCount,
            CreationTime = c.CreationTime,
            LeanerCount = c.LeanerCount,
            RegionStatus = c.RegionStatus
        };


        private Expression<Func<CourseEntity, CourseEntity>> OpenApiselector = c => new CourseEntity
        {
            ID = c.ID,
            Name = c.Name,
            CoverUrl = c.CoverUrl,
            SchoolName = c.SchoolName,
            SignatureName = c.SignatureName,
            Status = c.Status,
            SchoolCode = c.SchoolCode,
            RegionCode = c.RegionCode,
            CreatorCode = c.CreatorCode,
            CollbratorCount = c.CollbratorCount,
            CreationTime = c.CreationTime,
            LeanerCount = c.LeanerCount,
            RegionStatus = c.RegionStatus,
            CreatorName = c.CreatorName,
            Intro = c.Intro,
            CatalogName = c.CatalogName,
            CatalogId = c.CatalogId
        };




        //当前用户是学习身份，如果查询方式是c ,则以学生当前的学段进行排序
        private async Task<IQueryable<CourseEntity>> OrderByAsync(IQueryable<CourseEntity> source, char flag, char queryType, bool isOpenApi)
        {


            if (queryType == '0')
            {



                if (this._appUsr.IsLogin && this._appUsr.IsStudent)
                {

                    int section = (await this._appUsr.Get(AppUserFlagData.OrgSection)).Code;

                    source = source
                    .Select(c => new CourseEntity
                    {
                        ID = c.ID,
                        Name = c.Name,
                        CoverUrl = c.CoverUrl,
                        SchoolName = c.SchoolName,
                        SignatureName = c.SignatureName,
                        Status = c.Status,
                        RegionStatus = c.RegionStatus,
                        SchoolCode = c.SchoolCode,
                        RegionCode = c.RegionCode,
                        CreatorCode = c.CreatorCode,
                        Section = c.Section == section ? 100000 : 0,
                        CollbratorCount = c.CollbratorCount,
                        CreationTime = c.CreationTime,
                        LeanerCount = c.LeanerCount,
                    }).OrderByDescending(c => c.Section);

                }
                else
                {
                    if (isOpenApi)
                    {
                        source = source.Select(OpenApiselector);
                    }
                    else
                    {

                        source = source.Select(selector);
                    }
                }
            }





            if (flag == 'h')
            {
                source = source.OrderByDescending(C => C.LeanerCount);
            }
            else
            {
                source = source.OrderByDescending(c => c.CreationTime);
            }

            return source;


        }

        public async Task<Pagnation<CoursePagingVO>> QueryAsync(CourseParamters param, bool isOpenApi = false)
        {


            var query = await this.BuidQueryAsync(param);
            query = await this.OrderByAsync(query, param.OrderBy, param.QueryType, isOpenApi);




            var rev = await query
                   .ToPaginateAsync(param.PageIndex, param.PageSize);

            if (rev is null or { Count: <= 0 })
            {
                return null;
            }

            IList<CoursePagingVO> voItems = new List<CoursePagingVO>();


            //收集所有ID
            IList<string> cidAry = rev.Items.Select(v => v.ID).ToList();


            //查询所有协作者
            var collabrator = await this._reader.GetRepositoryAsync<CollabratorEntity>()
                                .Queryable(c => cidAry.Contains(c.CourseId))
                                .Select(c => new
                                {
                                    c.ObjId,
                                    c.CourseId
                                })
                                .ToListAsync();

            //循环赋值

            foreach (var item in rev.Items)
            {
                var vo = this._mp.Map<CoursePagingVO>(item);

                //vo.LearnerCount = item.LeanerCount;


                List<int> collabratorAry = collabrator.Where(c => c.CourseId == item.ID).Select(c => c.ObjId).ToList();

                await vo.SetOperationValAsync(new ValidateParameter(item.SchoolCode, item.RegionCode, item.CreatorCode, collabratorAry, (CourseStatus)item.Status, (CourseStatus)item.RegionStatus),
                                                this._pm,
                                                this._mp);

                if (vo.CoverUrl is null or {Length: <= 0 })
                {
                    vo.CoverUrl = "https://fcdata.forclass.net/PaasRes/Program/Test/Course/Images/cover1.png";
                }

                voItems.Add(vo);

                //vo.LearnerCount = item.LeanerCount;
            }


            return new Pagnation<CoursePagingVO>
            {
                ItemCount = rev.Count,
                Data = voItems
            };

        }



        //private async Task<(int, int)> GetReScCodeAsync()
        //{
        //    if (this._appUsr.IsLogin is false)
        //    {
        //        return (0, 0);
        //    }
        //    var sc =  await this._appUsr.GetSchool();
        //    var re = await this._appUsr.GetRegion();
        //    return (re.Code, sc.Code);
        //}

        private static string LikeWord(string word) => $"%{word}%";

        private async Task<IQueryable<CourseEntity>> BuidQueryAsync(CourseParamters param)
        {
            Expression<Func<CourseEntity, bool>> filter = _ => true; //= c => true;


            //关键词
            if (param.Keyword is not null and { Length: > 0 })
            {
                filter = filter.And(c => EF.Functions.Like(c.CreatorName, LikeWord(param.Keyword))
                                        || EF.Functions.Like(c.Name, LikeWord(param.Keyword))
                                        || EF.Functions.Like(c.SchoolName, LikeWord(param.Keyword))
                                        || EF.Functions.Like(c.RegionName, LikeWord(param.Keyword)));
            }

            //如果是学生只要加入到学习中就不再考虑状态，除非删除
            if (param.Status > 0)
            {

                if ((param.Status & (int)CourseStatus.Region) > 0)
                {
                    filter = c => c.RegionStatus == param.Status;
                }
                else
                {
                    filter = c => c.Status == param.Status;
                }

            }

            if (param.StartDate is not null and { Length: > 0 })
            {
                DateTime time = DateTime.Parse(param.StartDate);
                filter = filter.And(c => EF.Functions.DateDiffDay(time, c.CreationTime) >= 0);
            }
            if (param.EndDate is not null and { Length: > 0 })
            {
                DateTime time = DateTime.Parse(param.EndDate);
                filter = filter.And(c => EF.Functions.DateDiffDay(time, c.CreationTime) <= 0);


            }


            // 审核
            if (param.QueryType is 'a')
            {

                //if (param.OrgId <= 0)
                //{
                //    throw new CPValidateExceptions("参数'orgId'必须大于零");
                //}
                filter = await QueryTools.FilterByPermissionAsync(filter, _appUsr, _pm); //this.FilterByPermissionAsync(filter, param.OrgId);

            }



            //个人
            if (param.QueryType is 'c')
            {


                IQueryable<string> collabrator = this._reader.GetRepositoryAsync<CollabratorEntity>()
                        .Queryable(c => c.ObjId == this._appUsr.UserId)
                        .Select(c => c.CourseId);


                filter = filter.And(c => c.CreatorCode == this._appUsr.UserId || collabrator.Contains(c.ID));
            }

            //首页
            if (param.QueryType is '0')
            {
                if (param.OrgId <= 0)
                {
                    throw new CPValidateExceptions("参数'orgId'必须大于零");
                }
                if (param.OrgType is '0')
                {
                    throw new CPValidateExceptions("参数’org_type`不能为空");
                }

                var (re, sc) = await this._appUsr.GetReScCodeAsync();

                filter = QueryTools.QueryByAuthor(filter, this._appUsr.IsLogin, param.OrgType, param.OrgId, re, sc);

            }


            //Tag筛选查询....
            //var (exp, tagQuery) = this.TagFilterResult(filter, param.Tags);

            //filter = exp;
            var (tagQuery, createType) = filter.TagFilter(this._reader.GetRepositoryAsync<TagsEntity>(), param.Tags);

            //协作创建
            if (createType == ConstData.Tag_Collabrator_Create)
            {
                filter = filter.And(c => c.CollbratorCount > 0);
            }
            //个人创建
            if (createType == ConstData.Tag_Persional_Create)
            {
                filter = filter.And(c => c.CollbratorCount == 0);
            }

            var query = this._reader.GetRepositoryAsync<CourseEntity>()
                .Queryable(filter);

            // 学生
            if (param.QueryType is 's')
            {
                var joinedData = this._reader.GetRepositoryAsync<PlatformUserJoinedCourseEntity>()
                     .Queryable(p => p.UserId == this._appUsr.UserId)
                     .Select(p => p.CourseId)
                     .Distinct();
                query = from q in query
                        join j in joinedData
                        on q.ID equals j
                        select q;
            }


            if (tagQuery is not null)
            {
                query = from q in query
                        join t in tagQuery
                        on q.ID equals t
                        select q;
            }

            return query;
        }



        //根据权限设置查询参数
        //private async Task<Expression<Func<CourseEntity, bool>>> FilterByPermissionAsync(Expression<Func<CourseEntity, bool>> filter, int orgId)
        //{

        //    var re = await this._appUsr.Get(AppUserFlagData.OrgRegion);
        //    var sc = await this._appUsr.Get(AppUserFlagData.OrgSchool);


        //    if (this._pm.IsRegionAuditor)
        //    {
        //        filter = filter.And(c => /*c.CreatorCode == _appUsr.UserId || */c.RegionCode == re.Code/* orgId*/);
        //        filter = filter.And(c => (c.RegionStatus == (int)CourseStatus.RegionReview
        //        || c.RegionStatus == (int)CourseStatus.RegionListed
        //        || c.RegionStatus == (int)CourseStatus.RegionAccept
        //        || c.RegionStatus == (int)CourseStatus.RegionUnlisted));
        //    }
        //    else if (this._pm.IsSchoolAuditor)
        //    {
        //        filter = filter.And(c => /*c.CreatorCode == _appUsr.UserId ||*/ c.SchoolCode == sc.Code/* orgId*/);
        //        filter = filter.And(c => (c.Status == (int)CourseStatus.Listed
        //                                || c.Status == (int)CourseStatus.Review
        //                                || c.Status == (int)CourseStatus.Listed
        //                                || c.Status == (int)CourseStatus.Accept
        //                                || c.Status == (int)CourseStatus.UnListed));
        //    }
        //    else
        //    {

        //        throw new CPPermissionException();
        //        //filter = filter.And(c => c.CreatorCode == _appUsr.UserId);
        //    }
        //    return filter;
        //}



        // 标签查询后的课程总数...
        public async Task<int> TagFilterCount(/*IList<TagParam> tags, int creatorId*/CourseParamters param)
        {

            param.Status = 0;


            var query = await this.BuidQueryAsync(param);
            query = await this.OrderByAsync(query, param.OrderBy, param.QueryType, false);

            return await query.CountAsync();


            //if (tags is null or { Count: <= 0 })
            //{

            //    return Task.FromResult(0);
            //}
            //Expression<Func<CourseEntity, bool>> courseFilter = _ => true;

            //if (creatorId > 0)
            //{
            //    courseFilter = courseFilter.And(c => c.CreatorCode == creatorId);
            //}

            //var (filter, query) = this.TagFilterResult(courseFilter, tags);


            //var cq = this._reader.GetRepositoryAsync<CourseEntity>()
            //            .Queryable(filter)
            //            .Join(query, c => c.ID, t => t, (c, t) => c.ID)
            //            .Distinct()
            //            .CountAsync();

            //   return cq;


        }


        //// 过滤标签查询结果
        //public (Expression<Func<CourseEntity, bool>>, IQueryable<string>) TagFilterResult(Expression<Func<CourseEntity, bool>> source, IList<TagParam> tags)
        //{

        //    if (tags is null or { Count: <= 0 })
        //    {
        //        return (source, null);
        //    }


        //    HashSet<string> tagAry = new HashSet<string>();


        //    //标签


        //    foreach (var item in tags)
        //    {
        //        if (item.Name == ConstData.Tag_Persional_Create)
        //        {
        //            source = source.And(c => c.CollbratorCount == 0);
        //            continue;
        //        }
        //        if (item.Name == ConstData.Tag_Collabrator_Create)
        //        {
        //            source = source.And(c => c.CollbratorCount > 0);
        //            continue;
        //        }
        //        if (item.Name is null or { Length: <= 0 })
        //        {
        //            continue;
        //        }
        //        tagAry.Add(item.Name);

        //    }


        //    if (tagAry.Count is 0)
        //    {
        //        return (source, null);
        //    }

        //    Expression<Func<TagsEntity, bool>> tagFitler = _ => true;

        //    foreach (var item in tagAry)
        //    {
        //        tagFitler = tagFitler.And(t => t.Name == item);
        //    }


        //    var tagQuery = this._reader.GetRepositoryAsync<TagsEntity>()
        //            .Queryable(tagFitler)
        //            .Select(t => t.CourseId)
        //            .Distinct();


        //    return (source, tagQuery);

        //}

    }
}
