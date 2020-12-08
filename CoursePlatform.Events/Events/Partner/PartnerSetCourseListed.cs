using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events.Partner
{
    public record PartnerSetCourseListed(int Creator,int OrgId, string PltId,int ParentId, string OrgName,int Type, string CourseId, int Count) : ICoursePlatformEvent
    {
    }
}
