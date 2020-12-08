using CoursePlatform.infrastructure.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoursePlatform.Test
{
    public static class TestHelper
    {

        public static T Data<T>(this ReturnVal<T> ret)
        {
            if (ret.Result is null or {Count: <=0 })
            {
                return default;
            }
            return ret.Result.FirstOrDefault();
        }

    }
}
