using CoursePlatform.infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Modules.Filters
{
    public class LoginFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            string token = context.HttpContext.Request.Headers[CoursePlatformContext.HeaderToken];

            if (token is null or { Length: <= 0 })
            {
                throw new NotLoginException();
            }

            base.OnActionExecuting(context);
        }

    }
}
