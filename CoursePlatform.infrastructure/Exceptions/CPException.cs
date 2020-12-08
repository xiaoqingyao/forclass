using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoursePlatform.infrastructure.Exceptions
{
    public class CPException : Exception
    {

        ///// <summary>
        ///// 错误状态码
        ///// </summary>
        //public virtual int ErrCode  => -1;

        public CPException(string message) : base(message)
        {
            this.HResult = -1;
        }
    }
}
