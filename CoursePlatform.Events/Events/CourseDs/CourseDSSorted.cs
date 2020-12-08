using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events.Events
{
    public class CourseDSSorted : ICoursePlatformEvent
    {
            public IList<SortedData> Items { get; set; }

            
    }


    public class SortedData
    {
        public int CatalogId { get; set; }

        public string CourseId { get; set; }

        public Guid DsId { get; set; }

        public int SortVal { get; set; }
    }
}
