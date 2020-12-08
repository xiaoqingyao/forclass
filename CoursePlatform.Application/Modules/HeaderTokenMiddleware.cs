using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CoursePlatform.infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Modules
{
    public class HeaderTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            string tokent = context.Request.Headers["Author"];

            //if (String.IsNullOrEmpty(tokent))
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    throw new CPValidateExceptions("缺少Session信息..");
            //}

            return _next(context);
        }
    }


    public static class HeaderTokenMiddlewareExtenstions
    {
        public static void UseHeaderToken(this IApplicationBuilder app)
        {
            app.UseMiddleware<HeaderTokenMiddleware>();
        }
    }
}
