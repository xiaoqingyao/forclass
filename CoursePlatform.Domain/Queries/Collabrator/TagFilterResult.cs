using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.EFProvider.Paging;
using CoursePlatform.Data.Entities;
using CoursePlatform.Domain.DTOS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.Collabrator
{
    public interface ITagFilterResult
    {
    }

    public class TagFilterResult : ITagFilterResult
    {


        private readonly IUnitOfWork<CPDbContext> _reader;

        private HashSet<string> tagAry;// = new HashSet<string>();

        public TagFilterResult(IUnitOfWork<CPDbContext> reader)
        {
            _reader = reader;
            this.tagAry = new HashSet<string>();

        }

        public (Expression<Func<CourseEntity, bool>>, IQueryable<string>) Filter(Expression<Func<CourseEntity, bool>> source, IList<TagParam> tags)
        {

            if (tags is null or { Count: <= 0 })
            {
                return (source, null);
            }




            //标签


            foreach (var item in tags)
            {
                if (item.Name == ConstData.Tag_Persional_Create)
                {
                    source = source.And(c => c.CollbratorCount == 0);
                    continue;
                }
                if (item.Name == ConstData.Tag_Collabrator_Create)
                {
                    source = source.And(c => c.CollbratorCount > 0);
                    continue;
                }
                if (item.Name is null or { Length: <= 0 })
                {
                    continue;
                }
                tagAry.Add(item.Name);

            }


            if (tagAry.Count is 0)
            {
                return (source, null);
            }

            Expression<Func<TagsEntity, bool>> tagFitler = _ => true;

            foreach (var item in tagAry)
            {
                tagFitler = tagFitler.And(t => t.Name == item);
            }


            var tagQuery = this._reader.GetRepositoryAsync<TagsEntity>()
                    .Queryable(tagFitler)
                    .Select(t => t.CourseId)
                    .Distinct();


            return (source, tagQuery);

        }

    }
}
