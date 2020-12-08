using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoursePlatform.infrastructure.Validators
{
    public static class Prosecutor
    {

        public static bool NotNull(object obj, string msg = "对象为空")
        {
            if (obj == null)
            {
                throw new CPValidateExceptions(msg);
            }
            return true;
        }




        public static bool IsNull<T>(this T root, Expression<Func<T, object>> getter)
        {
            var visitor = new IsNullVisitor
            {
                CurrentObject = root
            };
            visitor.Visit(getter);
            return visitor.IsNull;
        }


        public static T Binder<T>(this T source) where T : class, new()
        {
            if (source == default(T))
            {
                source = new T();
            }
            return source;
        }


        public static AsyncLazy<T> Binder<T>(this AsyncLazy<T> source) where T : class, new()
        {
            if (source.Value == null)
            {
                source = new AsyncLazy<T>(new T());
            }
            return source;
        }

        public static bool NoData<T>(this IEnumerable<T> source)
        {
            return source ==  null || source.Count() == 0;
        }
    }
}
