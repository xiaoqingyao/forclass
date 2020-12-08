using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.Shared
{
    public enum CourseProp
    {
        Personal = 0x1,

        Collabrator =  Personal << 1

    }
}
