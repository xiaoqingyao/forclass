using CoursePlatform.Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Commands
{
    public record CreateCourseCommand(CreateCourseDTO dTO): CommandBase
    {
    }
}
