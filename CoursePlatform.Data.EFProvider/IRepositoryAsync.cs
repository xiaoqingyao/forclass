using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CoursePlatform.Data.EFProvider.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CoursePlatform.Data.EFProvider
{
    public interface IRepositoryAsync<T> where T : EntityBase,new() 
    {
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        //  IEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 2000000,
            bool disableTracking = true,
            CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddAsync(params T[] entities);

        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);


        void UpdateAsync(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predeicate);

        void DetachEntity(T entity);

        Task UpdateBach(Expression<Func<T, bool>> fitelr, Expression<Func<T, T>> expression);


        Task DelAsync(Expression<Func<T, bool>> fitelr);

        EntityState State(T entity);


        void SetState(T entity, EntityState state);


        Task<List<T>> Query(Expression<Func<T, bool>> fiter);

        IQueryable<T> Queryable(Expression<Func<T, bool>> filter);


        Task<int> Count(Expression<Func<T, bool>> filter);
    }
}
