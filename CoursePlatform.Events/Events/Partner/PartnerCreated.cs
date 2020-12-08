using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events.Partner
{
    public record PartnerCreated(int ResourceId, string Name, int Type) : ICoursePlatformEvent
    {
        public int ParentId { get; set; }
        public string Id { get; set; }
    }
}
