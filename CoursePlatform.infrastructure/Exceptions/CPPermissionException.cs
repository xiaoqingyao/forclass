using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure.Exceptions
{
    public class CPPermissionException : CPException
    {
        public CPPermissionException(string message = "无权执行此操作") : base(message)
        {
            this.HResult = 405;
        }
    }
}
