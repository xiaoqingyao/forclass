using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.infrastructure.Exceptions
{
    public class NotLoginException : CPException
    {
        public NotLoginException(string message = "未登录") : base(message)
        {
            this.HResult = 401;
        }


    }
}
