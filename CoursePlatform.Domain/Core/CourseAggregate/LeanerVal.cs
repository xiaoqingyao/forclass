using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.ValueFactory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.Core.CourseAggregate
{
    public class LeanerVal : ValueBase<int, LeanerItemVal>
    {
        //public HashSet<LeanerItemVal> Leaner { get; set; }

        //public bool Add(LeanerItemVal val)
        //{
        //    if (this.Leaner == null)
        //    {
        //        this.Leaner = new HashSet<LeanerItemVal>();
        //    }

        //    return this.Leaner.Add(val);
        //}

        //public bool Remove(LeanerItemVal val)
        //{
        //    if (this.Leaner == null)
        //    {
        //        return false;
        //    }

        //    return this.Leaner.RemoveWhere(l => l.UserId == val.UserId) > 0;
        //}

        //[JsonIgnore]
        //public int Count
        //{
        //    get
        //    {
        //        return this.Leaner == null ? 0 : this.Leaner.Count;
        //    }
        //}

    }

    public record LeanerItemVal(int Key, string PlatUserId) : ValItem<int>(Key) {



   }
}
