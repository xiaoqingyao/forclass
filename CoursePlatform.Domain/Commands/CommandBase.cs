using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Commands
{
    public  record  CommandBase
    {
        public string CommandId { get; set; }
    }
}
