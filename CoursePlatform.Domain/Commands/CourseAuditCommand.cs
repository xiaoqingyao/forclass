using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Commands
{
    public record CourseAuditCommand(string CourseId
        , int ReviewerId
        , string ReviewerName
        , int ReviewerOrgId
        , string ReviewerOrgName
        , string Desc):CommandBase
    {

    }
}
