using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CollabratorChanged : ICoursePlatformEvent
    {

        public string CourseId { get; set; }


        public int CollabratorCount { get; set; }

    }
}
