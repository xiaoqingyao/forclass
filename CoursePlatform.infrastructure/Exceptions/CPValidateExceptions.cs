using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure.Exceptions
{
    public class CPValidateExceptions : CPException
    {
        public CPValidateExceptions(string message) : base(message)
        {
        }
    }
}
