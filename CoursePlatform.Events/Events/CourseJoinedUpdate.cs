using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseJoinedUpdate : ICoursePlatformEvent
    {
        public string CourseId { get; set; }

        public int Number { get; set; }
    }
}
