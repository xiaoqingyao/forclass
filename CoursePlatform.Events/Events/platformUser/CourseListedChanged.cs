using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events.platformUser
{
    public record CourseListedChanged (int UserId, int Count): ICoursePlatformEvent
    {
    }
}
