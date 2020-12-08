using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events
{
    public record CourseBeAudited : ICoursePlatformEvent
    {

        public string CourseId { get; set; }

        public string ReviewerName { get; set; }

        public int ReviewerId { get; set; }

        public string Desc { get; set; }

        public int ReviewerOrgId { get; set; }

        public string ReviewerOrgName { get; set; }

        public int Status { get; set; }

        public string StatusDesc { get; set; }
    }
}
