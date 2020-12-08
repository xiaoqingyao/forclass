using CoursePlatform.Domain.Core.CourseAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Permission
{
    public record ValidateParameter(int SchoolId, int RegionId, int Creator, IList<int> Collabrator, CourseStatus SchoolStatus, CourseStatus RegionStatus)
    {
    }
}
