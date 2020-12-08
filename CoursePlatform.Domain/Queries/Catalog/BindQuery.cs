using CoursePlatform.Asset;
using CoursePlatform.Asset.ApiDTO;
using CoursePlatform.Data;
using CoursePlatform.Data.EFProvider;
using CoursePlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.Catalog
{


    public interface IBindQuery
    {
        Task<IList<CatalogResult>> GetAsync(QueryParam param);
    }


    public class BindQuery : IBindQuery
    {

        private readonly IUnitOfWork<CPDbContext> _reader;

        private readonly ICatalogProxy _proxy;

        public BindQuery(IUnitOfWork<CPDbContext> reader, ICatalogProxy proxy)
        {
            _reader = reader;
            _proxy = proxy;
        }

        public async Task<IList<CatalogResult>> GetAsync(QueryParam param)
        {
            var result = await this._proxy.GetAsync(param.Param);

            if (result is null or { Count: <= 0 })
            {
                return null;
            }

            var unitItem = result.FirstOrDefault(r => r.Name == "单元");
            if (unitItem is null)
            {
                return null;
            }
            if (unitItem.ChildList is null or { Count: <= 0 })
            {
                return null;
            }


            var dsitems = await this.CourseCount(param.CourseId);

            if (dsitems is null or { Count: <= 0 })
            {
                return unitItem.ChildList;
            }

            foreach (var item in unitItem.ChildList)
            {
                item.Count += dsitems.Count(d => d == item.Id);  //await this.CourseCount(item.Id);

                if (item.ChildList is null or { Count: <= 0 })
                {
                    continue;
                }
                foreach (var first in item.ChildList)
                {
                    first.Count += dsitems.Count(d => d == first.Id);//await this.CourseCount(first.Id);

                    if (first.ChildList is not null and { Count: > 0 })
                    {

                        foreach (var secend in first.ChildList)
                        {
                            secend.Count += dsitems.Count(d => d == secend.Id); //await this.CourseCount(secend.Id);
                            //first.Count += secend.Count;
                        }
                    }

                    //item.Count += first.Count;
                }
            }

            return unitItem.ChildList;

        }


        //public IList<CatalogResult> Foreach(IList<CatalogResult> source,Action<CatalogResult> first, Action<CatalogResult, CatalogResult> second, Action<CatalogResult, CatalogResult, CatalogResult> third)
        //{
        //    foreach (var item in source)
        //    {
        //        first(item);

        //        if (item.ChildList is null or { Count: <= 0 })
        //        {
        //            continue;
        //        }
        //        foreach (var firstItem in item.ChildList)
        //        {
        //            second(item, firstItem);

        //            if (firstItem.ChildList is null or { Count: <= 0 })
        //            {
        //                continue;
        //            }
        //            foreach (var secondItem in firstItem.ChildList)
        //            {
        //                third(item, firstItem, secondItem);
        //            }
        //        }
        //    }
        //    return source;
        //}

        private Task<List<int>> CourseCount(string courseId)
        {
            return this._reader.GetRepositoryAsync<QuoteDsEntity>()
                .Queryable(c => c.CourseId == courseId)
                .Select(q => q.CatalogId)
                .ToListAsync();


        }
    }
}
