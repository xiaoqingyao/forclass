using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class TagUpdated : ICoursePlatformEvent
    {


        public string CourseId { get; set; }

        public TagEventVal Val { get; set; }

        public int SchoolId { get; set; }


        public int RegtionId { get; set; }

        public int CreatorId { get; set; }
        public string SchoolName { get; set; }
        public string RegionName { get; set; }
    }
}
