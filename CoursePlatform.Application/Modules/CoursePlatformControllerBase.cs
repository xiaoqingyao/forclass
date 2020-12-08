using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;
using CoursePlatform.infrastructure.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.Application.Modules
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CoursePlatformControllerBase : ControllerBase
    {


        protected readonly IAppUser _user;
        protected readonly IMapper _mapper;


        protected CoursePlatformControllerBase(IAppUser user, IMapper mapper)
        {
            _user = user;
            _mapper = mapper;
        }

        protected const string SwgTag_Qeury = "查询";
        protected const string SwgTag_Manager = "课程管理";
        protected const string SwgTag_User = "平台用户";
        protected const string SwgTag_Tools = "工具类";
        protected const string SwgTag_OpenAPI = "OpenApi";


        protected ReturnVal<T> RetOk<T>(T data)
        {
            return new ReturnVal<T>
            {
                Result = new List<T>
               {
                   data
               }
            };
        }

        public async Task<ReturnVal<T>> RetOkAsync<T>(Task<T> data)
        {
            var retData = await data;

            return RetOk(retData);
        }


        protected ReturnVal<T> RetErr<T> (string msg, int code = -1)
        {
            return new ReturnVal<T>
            {
                ReturnText = msg,
                ReturnCode = code
            };
        }

        
    

    }
}
