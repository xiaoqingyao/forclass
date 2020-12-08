using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseDsDeleted : ICoursePlatformEvent
    {

        public string CourseId { get; set; }


        public int CatalogId { get; set; }


        public Guid DsId { get; set; }
    }
}
