using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CoursePlatform.Data.EFProvider.Paging;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CoursePlatform.Data.EFProvider
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T :EntityBase, new()
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public RepositoryAsync(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) {
                predicate = predicate.And(t => t.Deleted == 0);
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync().ConfigureAwait(false);

            }


            var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);

            return result;

        }

        public Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 2020,
            bool disableTracking = true,
            CancellationToken cancellationToken = default) //default(CancellationToken))
        {

            if (index < 0)
            {
                index = 0;
            }


            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null)
            {
                predicate = predicate.And(t => t.Deleted == 0);
                query = query.Where(predicate);
            }

            if (orderBy != null)
                return orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);
            return query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default/*(CancellationToken)*/)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task AddAsync(params T[] entities)
        {
            return _dbSet.AddRangeAsync(entities);
        }


        public Task AddAsync(IEnumerable<T> entities,
            CancellationToken cancellationToken = default/*(CancellationToken)*/)
        {
            return _dbSet.AddRangeAsync(entities, cancellationToken);
        }


        //[Obsolete("Use get list ")]
        //public IEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public void UpdateAsync(T entity)
        {

            entity.UpdateTime = DateTime.Now;

            _dbSet.Update(entity);
        }

        public Task AddAsync(T entity)
        {
            entity.CreationTime = DateTime.Now;

            return AddAsync(entity, new CancellationToken());
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predeicate)
        {
            return _dbSet.AnyAsync(predeicate);
        }

        public void DetachEntity(T entity)
        {
            this._dbContext.Entry<T>(entity).State = EntityState.Detached;
            //throw new NotImplementedException();
        }

        public EntityState State(T entity)
        {
            return this._dbContext.Entry<T>(entity).State;
        }



        public Task UpdateBach(Expression<Func<T, bool>> fitelr, Expression<Func<T, T>> expression)
        {


            return this._dbSet.Where(fitelr).BatchUpdateAsync(expression);
        }

        public Task UpdateBach(Expression<Func<T, bool>> fitelr, T data)
        {
            data.UpdateTime = DateTime.Now;

            return this._dbSet.Where(fitelr).BatchUpdateAsync(data);
        }

        public void SetState(T entity, EntityState state)
        {
            this._dbContext.Entry<T>(entity).State = state;
        }


        public Task<List<T>> Query(Expression<Func<T, bool>> fiter)
        {
            fiter = fiter.And(f => f.Deleted == 0);

            return this._dbSet.Where(fiter).ToListAsync();
        }

        public IQueryable<T> Queryable(Expression<Func<T, bool>> filter)
        {
            filter = filter.And(f => f.Deleted == 0);

            return this._dbSet.Where(filter);
        }

        public Task<int> Count(Expression<Func<T, bool>> filter)
        {
            filter = filter.And(f => f.Deleted == 0);

            return this._dbSet.CountAsync(filter);
        }

        public Task DelAsync(Expression<Func<T, bool>> fitelr)
        {
            return this._dbSet.Where(fitelr).BatchUpdateAsync(t => new T { Deleted = 1, UpdateTime = DateTime.Now });
        }
    }


    //public static class RepositoryExtentions
    //{
    //    public static Task UpAsync<TSource>(this IRepositoryAsync<TSource> source,Expression<Func<TSource, bool>> filter,TSource data) where TSource : EntityBase, new()
    //    {

    //        //var input = (MemberInitExpression)setter.Body;

    //        //var updatePro = GetPro("UpdateTime", typeof(TSource));

    //        //var bindings = new List<MemberBinding>();

    //        //var time = DateTime.Now;

    //        //bindings.Add(Expression.Bind(updatePro, Expression.Constant(time)));

    //        //foreach (var item in input.Bindings)
    //        //{
    //        //    bindings.Add(item);
    //        //}

    //        //var memberInit = Expression.MemberInit(Expression.New(typeof(TSource)), bindings.ToArray());


    //        //setter = Expression.Lambda<Func<TSource,TSource>>(memberInit);
            
    //        setter.Compile().

    //        return source.UpdateBach(filter, data);
    //    }


    //    private static IDictionary<string, PropertyInfo[]> propertyDict = new Dictionary<string, PropertyInfo[]>();


    //    private static PropertyInfo GetPro(string name, Type source)
    //    {
    //        if (propertyDict.TryGetValue(source.FullName, out PropertyInfo[] values))
    //        {
    //            return values.FirstOrDefault(f => f.Name == name);
    //        }

    //        values = source.GetProperties();

    //        propertyDict.Add(source.FullName, values);


    //        return values.FirstOrDefault(f => f.Name == name);
    //    }
    //} 
}
