using Microsoft.AspNetCore.Http;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlatform.Application.Modules
{
    public class CoursePlatformContext : ICoursePlatformHttpContext
    {

        private string session;

        private IHttpContextAccessor _http;


        public const string HeaderToken = "Author";

        public CoursePlatformContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string Session
        {
            get
            {
                if (String.IsNullOrEmpty(session))
                {
                    session = _http.HttpContext.Request.Headers[HeaderToken].SingleOrDefault();
                }
                //if (String.IsNullOrEmpty(session))
                //{
                //    throw new CPValidateExceptions("session token lost");
                //}
                return session;
            }
        }//throw new NotImplementedException();
    }
}
