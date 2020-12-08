using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.EFProvider.Paging;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.Domain.DTOS.Queries;
using CoursePlatform.Domain.Permission;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries
{
    public static class QueryTools
    {

        public static Expression<Func<CourseEntity, bool>> QueryByAuthor(Expression<Func<CourseEntity, bool>> subFiler
            , bool isLogin
            , char orgType
            , int orgId
            , int userRegtionId
            , int userSchoolId)
        {


            if (isLogin)
            {
                if (orgType is ConstData.Filter_SchoolOrg)
                {
                    if (userSchoolId == orgId)
                    {

                        subFiler = subFiler.And(c => c.Status == (int)CourseStatus.Accept || c.Status == (int)CourseStatus.Listed);
                    }
                    else
                    {

                        subFiler = subFiler.And(c => c.Status == (int)CourseStatus.Listed);
                    }

                    subFiler = subFiler.And(c => c.SchoolCode == orgId);
                }
                else
                {
                    if (userRegtionId == orgId)
                    {

                        subFiler = subFiler.And(c => c.RegionStatus == (int)CourseStatus.RegionAccept || c.RegionStatus == (int)CourseStatus.RegionListed);
                    }
                    else
                    {

                        subFiler = subFiler.And(c => c.RegionStatus == (int)CourseStatus.RegionListed);
                    }
                    subFiler = subFiler.And(c => c.RegionCode == orgId);
                }
            }
            else
            {
                if (orgType is ConstData.Filter_SchoolOrg)
                {
                    subFiler = subFiler.And(c => c.Status == (int)CourseStatus.Listed);
                    subFiler = subFiler.And(c => c.SchoolCode == orgId);
                }
                else
                {
                    subFiler = subFiler.And(c => c.RegionStatus == (int)CourseStatus.RegionListed);
                    subFiler = subFiler.And(c => c.RegionCode == orgId);
                }

            }

            return subFiler;
        }

        // 过滤标签查询结果
        public static (IQueryable<string>, string) TagFilter(this Expression<Func<CourseEntity, bool>> source, IRepositoryAsync<TagsEntity> tagRes, IList<TagParam> tags)
        {

            if (tags is null or { Count: <= 0 })
            {
                return (null, null);
            }




            var groupTag = tags.GroupBy(t => t.TypeName);


            string creatType = null;



            //创建
            var createTag = groupTag.SingleOrDefault(c => c.Key == ConstData.Tag_Create);

            if (createTag is not null)
            {
                if (createTag.Count() == 1)
                {
                    creatType = createTag.First().Name;
                }
            }

            //其它
            var otherTag = groupTag.Where(o => o.Key is not ConstData.Tag_Create);


            if (otherTag.NoData())
            {
                return (null, creatType);
            }


            IQueryable<string> otherQuery = null ;


            otherQuery = otherTag.Aggregate(otherQuery, (oq, next) =>
              {

                  var bq = BindQueryTag(tagRes, next);

                  if (oq == null)
                  {
                      oq = bq;
                  }
                  else
                  {
                      oq = oq.Intersect(bq);
                  }

                  return oq;
              });

        


            var endQuery = otherQuery.Select(t => t)
                .Distinct();


            return (endQuery, creatType);


          
        }


        private static IQueryable<string> BindQueryTag(IRepositoryAsync<TagsEntity> tagRes, IGrouping<string, TagParam> group)
        {
            var names = group.AsEnumerable().Select(t => t.Name);

            string type = group.Key == ConstData.Tag_Course ? null : group.Key;

            return tagRes.Queryable(t => t.TypeName == type && names.Contains(t.Name)).Select(t => t.CourseId).Distinct();
        }



        public static async Task<(int, int)> GetReScCodeAsync(this IAppUser appUsr)
        {
            if (appUsr.IsLogin is false)
            {
                return (0, 0);
            }
            var sc = await appUsr.GetSchool();
            var re = await appUsr.GetRegion();
            int schoolId = sc is null ? 0 : sc.Code;
            return (re.Code, schoolId);
        }



        public static async Task<Expression<Func<CourseEntity, bool>>> FilterByPermissionAsync(Expression<Func<CourseEntity, bool>> filter, IAppUser appUsr, IPermission pm)
        {

            //var re = await appUsr.Get(AppUserFlagData.OrgRegion);
            //var sc = await appUsr.Get(AppUserFlagData.OrgSchool)j;

            var (rc, sc) = await appUsr.GetReScCodeAsync();

            if (pm.IsRegionAuditor)
            {
                filter = filter.And(c => /*c.CreatorCode == _appUsr.UserId || */c.RegionCode == rc/* orgId*/);
                filter = filter.And(c => (c.RegionStatus == (int)CourseStatus.RegionReview
                || c.RegionStatus == (int)CourseStatus.RegionListed
                || c.RegionStatus == (int)CourseStatus.RegionAccept
                || c.RegionStatus == (int)CourseStatus.RegionUnlisted));
            }
            else if (pm.IsSchoolAuditor)
            {
                filter = filter.And(c => /*c.CreatorCode == _appUsr.UserId ||*/ c.SchoolCode == sc/* orgId*/);
                filter = filter.And(c => (c.Status == (int)CourseStatus.Listed
                                        || c.Status == (int)CourseStatus.Review
                                        || c.Status == (int)CourseStatus.Listed
                                        || c.Status == (int)CourseStatus.Accept
                                        || c.Status == (int)CourseStatus.UnListed));
            }
            else
            {

                throw new CPPermissionException();
                //filter = filter.And(c => c.CreatorCode == _appUsr.UserId);
            }
            return filter;
        }


    }
}
