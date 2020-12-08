using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure.Exceptions
{
    public class CPNotFoundException : CPException
    {
        public CPNotFoundException(string message) : base(message)
        {
        }
    }
}
