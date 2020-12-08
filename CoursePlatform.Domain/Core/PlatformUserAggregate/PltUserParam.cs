using CoursePlatform.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.PlatformUserAggregate
{
    public record PltUserParam(int UserId, string Name, int SchoolId, int RegionId, string SchoolName/*, UserPropVal ResearchGroup*/)
    {
        
    }
}
