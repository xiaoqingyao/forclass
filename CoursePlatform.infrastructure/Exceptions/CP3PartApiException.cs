using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure.Exceptions
{
    public class CP3PartApiException : CPException
    {
        public CP3PartApiException(string message) : base(message)
        {
        }
    }
}
