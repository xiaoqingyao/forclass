using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events.Partner
{
    public record PartnerCourseListRemoved(int ObjId, int ObjType,int Count,string CourseId) : ICoursePlatformEvent
    {
    }
}
