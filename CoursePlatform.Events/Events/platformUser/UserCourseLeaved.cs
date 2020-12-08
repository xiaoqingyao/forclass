using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events
{
    public class UserCourseLeaved : ICoursePlatformEvent
    {

        public string CourseId { get; set; }
        public int UserId { get; set; }

        public int CreatorId { get; set; }
    }
}
