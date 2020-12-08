using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CollabratorReplaced : ICoursePlatformEvent
    {

        public string CourseId { get; set; }


        public IList<CollabratorEventItem>  Items { get; set; }
    }
}
