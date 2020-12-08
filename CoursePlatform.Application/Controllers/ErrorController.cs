using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoursePlatform.infrastructure;
using CoursePlatform.infrastructure.Exceptions;

namespace CoursePlatform.Application.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {


        private readonly ILogger<ErrorController> _logger;
        private readonly IAppUser _user;

        public ErrorController(ILogger<ErrorController> logger, IAppUser user)
        {
            _logger = logger;
            _user = user;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("api/error"), HttpGet, HttpPost]
        public ApiResponseAsync<bool> Index(string error)
        {


            this.Response.StatusCode = (int)HttpStatusCode.OK;

            var err = HttpContext
                .Features
                .Get<IExceptionHandlerPathFeature>();
            if (err == null)
            {
                return new ApiResponseAsync<bool>()
                {
                    Code = -1,
                    Data = false,
                    Message = "系统异常"

                };
            }


            string message;

            // 已知异常返回具体信息
            if (err.Error is CPException)
            {
                message = err.Error.Message;
            }

            else
            {
                // 未知异常，返回普通信息
                message = "系统异常";
                _logger.LogError(err.Error, $"用户:{this._user.UserName}, 请求路径:{err.Path}");

            }



            return new ApiResponseAsync<bool>
            {
                Code = err.Error.HResult, //errCode,
                Data = false,
                Message = message
            };
        }




    }
}
