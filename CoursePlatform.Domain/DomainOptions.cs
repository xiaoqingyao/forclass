using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain
{
    public class DomainOptions
    {

        public const string OptionsSection = "DomainOptions";

        public string CourseCachePrefix { get; set; }

        public string CachePrefix { get; set; } = "CoursePlatform_Key";



        public string UserCachePrifex { get; set; } = "CourseUser_0_";

        public string PartnerCachePrefix { get; set; }

        public IList<int> SchoolAuditor { get; set; }


        public IList<int> RegionAuditor { get; set; }


        public IList<string> Filter { get; set; }


        public IDictionary<string, CourseOperationItem> CourseOperation { get; set; }

    }


    public class CourseOperationItem
    {
    
        public string Text { get; set; }

        public string URL { get; set; }

    }



}
