using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseDsStatusUpdated : ICoursePlatformEvent
    {
       public int CatalogId { get; set; }


        public string CourseId { get; set; }


        public Guid DsId { get; set; }


        public bool IsOpen { get; set; }
    }
}
