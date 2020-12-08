using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events.platformUser
{
    public record PltUserCourseCountChanged(int User, int Count) : ICoursePlatformEvent
    {
    }
}
