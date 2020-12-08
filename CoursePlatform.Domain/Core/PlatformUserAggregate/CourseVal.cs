using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.ValueFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.PlatformUserAggregate
{
    public class OwnerCourseVal : ValueBase<string, OwnerCourseItemVal>
    {

    }

    public record OwnerCourseItemVal(string Key) : ItemVal(Key)
    {

    }
}
