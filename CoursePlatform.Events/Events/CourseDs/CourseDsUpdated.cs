using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events.Events
{
    public record CourseDsUpdated(string CoureId, int CatalogId, Guid DsId, bool IsOpen, bool IsShared, string Name, string CoverUrl) : ICoursePlatformEvent
    {
    }
}
