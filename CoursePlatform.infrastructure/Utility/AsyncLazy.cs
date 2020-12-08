using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoursePlatform.infrastructure.Utility
{
    /// <summary>
    ///     Provides support for lazy initialization in asyncronous manner.
    /// </summary>
    /// <typeparam name="T"> Specifies the type of object that is being lazily initialized. </typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AsyncLazy<T> : Lazy<T>
    {

        public AsyncLazy() :base()
        {

        }

        public AsyncLazy(T data) :base(data)
        {

        }

        public AsyncLazy(Func<Task<T>> factory) : base(() => Task.Run(factory).GetAwaiter().GetResult())
        {

        }


    }
}
