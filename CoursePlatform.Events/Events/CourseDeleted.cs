using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events
{
    public record CourseDeleted(string CourseId, int OperatorId): ICoursePlatformEvent
    {

    }
}
