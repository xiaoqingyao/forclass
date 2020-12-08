using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events.Collabrator
{
    public record CollabratorAllDeleted(string CourseId): ICoursePlatformEvent
    {
        
    }
}
