using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public record CourseSchoolStatusChanged(string CouserId, int Status) : ICoursePlatformEvent
    {
    }
}
