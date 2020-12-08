using CoursePlatform.Events;
using CoursePlatform.infrastructure.Utility;
using CoursePlatform.Infrastructure.Caching;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.ValueFactory
{
    public interface IValueFactory
    {
        void Register<TRoot, T, TVal>(TRoot root, Expression<Func<TRoot, T>> prop, Func<Task<TVal>> source)
            where TRoot : AggregateRoot
            where T : AsyncLazy<TVal>;
        bool Save<TRoot, T>(TRoot root, Expression<Func<TRoot, AsyncLazy<T>>> source) where TRoot : AggregateRoot;
    }


    public class ValueFactory : IValueFactory
    {


        private readonly ICachingProvider _cacheProvider;

        private readonly IOptions<DomainOptions> _opt;

        public ValueFactory(ICachingProvider cacheProvider, IOptions<DomainOptions> opt)
        {
            _cacheProvider = cacheProvider;
            _opt = opt;
        }

        public void Register<TRoot, T, TVal>(TRoot root, Expression<Func<TRoot, T>> prop, Func<Task<TVal>> source)
            where TRoot : AggregateRoot
            where T : AsyncLazy<TVal>
        {
            var memberExpres = (MemberExpression)prop.Body;

            var pro = GetPro(memberExpres.Member.Name, typeof(TRoot));

            string key = String.Concat(_opt.Value.CachePrefix, '-', typeof(TRoot).Name, '-', root.ID);


            pro.SetValue(root, new AsyncLazy<TVal>(async () =>
            {


                //string filed = typeof(TVal).Name;

                var ret = this._cacheProvider.HGet<TVal>(key, pro.Name);

                if (ret is null)
                {
                    ret = await source();
                }
                if (ret is null)
                {

                    return default;
                }

                this._cacheProvider.HSet(key, pro.Name, ret);

                return ret;

            }));


        }



        public bool Save<TRoot, T>(TRoot root, Expression<Func<TRoot, AsyncLazy<T>>> source)
           where TRoot : AggregateRoot
        {
            var member = (MemberExpression)source.Body;



            var pro = GetPro(member.Member.Name, typeof(TRoot));


            string key = String.Concat(_opt.Value.CachePrefix, '-', typeof(TRoot).Name, '-', root.ID);

            if (pro.GetValue(root) is not AsyncLazy<T> val)
            {

                var ret = this._cacheProvider.HDel(key, member.Member.Name);

                return ret > 0;
            }

            return this._cacheProvider.HSet(key, member.Member.Name, val.Value);

        }






        private static IDictionary<string, PropertyInfo[]> propertyDict = new Dictionary<string, PropertyInfo[]>();


        private static PropertyInfo GetPro(string name, Type source)
        {
            if (propertyDict.TryGetValue(source.FullName, out PropertyInfo[] values))
            {
                return values.FirstOrDefault(f => f.Name == name);
            }

            values = source.GetProperties();

            propertyDict.Add(source.FullName, values);


            return values.FirstOrDefault(f => f.Name == name);
        }

    }
}
